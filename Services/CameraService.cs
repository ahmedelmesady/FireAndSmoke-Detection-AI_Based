using Microsoft.AspNetCore.SignalR;
using OpenCvSharp;
using System;
using System.Threading;
using WebApplication1.Hubs;

namespace WebApplication1.Services
{
    public class CameraService
    {
        public  readonly IConfiguration Configuration;
        private readonly IHubContext<AlertHub> hubContext;
        private readonly IAIService AIService;
        private readonly IAlertService AlertService;
        private Mat? _latestFrame;
        private readonly object _lock = new();

        public CameraService(IAIService aIService, IConfiguration configuration, IAlertService alertService, IHubContext<AlertHub> hubContext)
        {
            AIService = aIService;
            Configuration = configuration;
            AlertService = alertService;
            this.hubContext = hubContext;
        }

        public async Task StartCameraAsync()
        {
            string? rtspUrl = Configuration.GetValue<string>("CameraSettings:RtspUrl");

            Environment.SetEnvironmentVariable(
                "OPENCV_FFMPEG_CAPTURE_OPTIONS",
                "rtsp_transport;tcp|fflags;nobuffer|flags;low_delay|max_delay;0");

            var capture = new VideoCapture(rtspUrl, VideoCaptureAPIs.FFMPEG);
            if (!capture.IsOpened())
                throw new Exception("Cannot connect to camera");

            capture.Set(VideoCaptureProperties.BufferSize, 1);

            _ = Task.Run(() =>
            {
                using var frame = new Mat(); 
                while (true)
                {
                    if (capture.Read(frame) && !frame.Empty())
                    {
                        lock (_lock)
                        {
                            _latestFrame?.Dispose();
                            _latestFrame = frame.Clone();
                        }
                    }
                    else
                    {
                        Thread.Sleep(200);
                    }
                }
            });

            while (true)
            {
                Mat? frameToAnalyze = null;
                lock (_lock)
                {
                    if (_latestFrame != null)
                        frameToAnalyze = _latestFrame.Clone();
                }

                if (frameToAnalyze != null)
                {
                    using (frameToAnalyze)  
                    {
                        Cv2.Resize(frameToAnalyze, frameToAnalyze, new Size(640, 480));
                        Cv2.ImEncode(".jpg", frameToAnalyze, out byte[] imageBytes);

                        var result = await AIService.PredictAsync(imageBytes);
                        if (result != null)
                        {
                            await AlertService.HandlePredictionAsync(result);
                            await hubContext.Clients.All.SendAsync("RecieveDetection",
                                new
                                {
                                    ImageUrl = result.ImageUrl,
                                    boxes = result.Boxes
                                });
                        }
                    }
                }

                await Task.Delay(500);
            }
        }
    }
}