namespace QuanLyBaoHiem.Services
{
    public class JobWorker : BackgroundService
    {
        private readonly JobQueueService _jobQueue;
        private readonly IServiceProvider _serviceProvider;

        public JobWorker(
            JobQueueService jobQueue,
            IServiceProvider serviceProvider)
        {
            _jobQueue = jobQueue;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Kiểm tra có công việc trong hàng đợi không
                if (_jobQueue.TryDequeue(out var job)
                    && job != null)
                {
                    try
                    {
                        // Thực hiện công việc
                        await job(_serviceProvider);
                    }
                    catch (Exception ex)
                    {
                        // Hiện lỗi Job trong Output
                        Console.WriteLine(
                            "Job bị lỗi: " + ex.Message);
                    }
                }

                // Nghỉ 1 giây rồi kiểm tra tiếp
                await Task.Delay(
                    TimeSpan.FromSeconds(1),
                    stoppingToken);
            }
        }
    }
}