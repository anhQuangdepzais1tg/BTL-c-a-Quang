using System.Collections.Concurrent;

namespace QuanLyBaoHiem.Services
{
    public class JobQueueService
    {
        // Hàng đợi công việc
        private readonly ConcurrentQueue<
            Func<IServiceProvider, Task>
        > _jobs = new();

        // Thêm công việc vào hàng đợi
        public void Enqueue(
            Func<IServiceProvider, Task> job)
        {
            _jobs.Enqueue(job);
        }

        // Lấy công việc tiếp theo
        public bool TryDequeue(
            out Func<IServiceProvider, Task>? job)
        {
            return _jobs.TryDequeue(out job);
        }
    }
}