using ClubOverdose.Data;
using ClubOverdose.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClubOverdose.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var now = DateTime.Now;

            var recentReservationItems = await _context.Reservations
                .AsNoTracking()
                .Include(r => r.Client)
                .Include(r => r.Event)
                .OrderByDescending(r => r.ReservationDate)
                .Take(5)
                .ToListAsync();

            var recentReservations = recentReservationItems
                .Select(r =>
                {
                    var clientName = $"{r.Client.FirstName} {r.Client.LastName}".Trim();

                    if (string.IsNullOrWhiteSpace(clientName))
                    {
                        clientName = r.Client.UserName ?? r.ClientId;
                    }

                    return new AdminRecentReservationViewModel
                    {
                        Id = r.Id,
                        ClientName = clientName,
                        EventName = r.Event.Name,
                        ReservationDate = r.ReservationDate,
                        NumberOfGuests = r.NumberOfGuests
                    };
                })
                .ToList();

            var upcomingEvents = await _context.Events
                .AsNoTracking()
                .Where(e => e.EventDateTime >= now)
                .OrderBy(e => e.EventDateTime)
                .Take(4)
                .Select(e => new AdminUpcomingEventViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    ImageUrl = e.ImageUrl,
                    EventDateTime = e.EventDateTime,
                    ReservationCount = e.Reservations.Count
                })
                .ToListAsync();

            var model = new AdminDashboardViewModel
            {
                TotalEvents = await _context.Events.CountAsync(),
                TotalReservations = await _context.Reservations.CountAsync(),
                TotalDrinks = await _context.Drinks.CountAsync(),
                TotalMenus = await _context.Menus.CountAsync(),
                TotalTypes = await _context.Types.CountAsync(),
                TotalUsers = await _context.Users.CountAsync(),
                UpcomingEvents = await _context.Events.CountAsync(e => e.EventDateTime >= now),
                UpcomingEventReservations = await _context.Reservations.CountAsync(r => r.Event.EventDateTime >= now),
                RecentReservations = recentReservations,
                UpcomingEventItems = upcomingEvents
            };

            return View(model);
        }
    }
}
