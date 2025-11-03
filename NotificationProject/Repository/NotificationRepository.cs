using Microsoft.EntityFrameworkCore;
using NotificationProject.Data;
using NotificationProject.Models;
using NotificationProject.Repository.IRepository;

namespace NotificationProject.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _db;

        public NotificationRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Notification> AddAsync(Notification notification)
        {
            await _db.Notifications.AddAsync(notification);
            await _db.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _db.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> QueryAsync(int? userId = null, int skip = 0, int take = 50)
        {
            var q = _db.Notifications.AsQueryable();
            q = userId != null ? q = q.Where(x => x.UserId == userId) : q;
            var x = await q.OrderByDescending(u => u.CreatedAt).Skip(skip).Take(take).ToListAsync();
            return await q.OrderByDescending(u => u.CreatedAt).Skip(skip).Take(take).ToListAsync();
        }

        public async Task UpdateAsync(Notification notification)
        {
            _db.Notifications.Update(notification);
            await _db.SaveChangesAsync();
        }
    }
}
