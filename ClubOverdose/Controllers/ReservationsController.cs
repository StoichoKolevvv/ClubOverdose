using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClubOverdose.Data;
using ClubOverdose.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ClubOverdose.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Client> _userManager;

        public ReservationsController(ApplicationDbContext context, 
                                      UserManager<Client> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservations
        public async Task<IActionResult> Index(int? eventId)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");

            if (isSuperAdmin)
            {
                if (eventId == null)
                {
                    var events = await _context.Events
                        .AsNoTracking()
                        .OrderBy(e => e.EventDateTime)
                        .ThenBy(e => e.Name)
                        .Select(e => new EventReservationSummaryViewModel
                        {
                            Event = e,
                            ReservationCount = e.Reservations.Count
                        })
                        .ToListAsync();

                    return View(new ReservationsIndexViewModel
                    {
                        IsSuperAdmin = true,
                        Events = events
                    });
                }

                var selectedEvent = await _context.Events
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == eventId.Value);

                if (selectedEvent == null)
                {
                    return NotFound();
                }

                var eventReservations = await _context.Reservations
                    .AsNoTracking()
                    .Include(r => r.Client)
                    .Include(r => r.Event)
                    .Where(r => r.EventId == eventId.Value)
                    .OrderByDescending(r => r.ReservationDate)
                    .ToListAsync();

                return View(new ReservationsIndexViewModel
                {
                    IsSuperAdmin = true,
                    SelectedEventId = selectedEvent.Id,
                    SelectedEventName = selectedEvent.Name,
                    SelectedEventDateTime = selectedEvent.EventDateTime,
                    Reservations = eventReservations
                });
            }

            var currentUserId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Challenge();
            }

            var userReservations = await _context.Reservations
                .AsNoTracking()
                .Include(r => r.Client)
                .Include(r => r.Event)
                .Where(r => r.ClientId == currentUserId)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();

            return View(new ReservationsIndexViewModel
            {
                Reservations = userReservations
            });
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create(int? eventId)
        {
            //ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", eventId);
            return View(new Reservation { EventId = eventId ?? 0 });
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,NumberOfGuests")] Reservation reservation)
        {
            reservation.ReservationDate = DateTime.Now;
            reservation.ClientId = _userManager.GetUserId(User)!;
            ClearServerManagedReservationValidation();

            if (ModelState.IsValid)
            {
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Reservation created successfully";
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", reservation.ClientId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", reservation.EventId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            //ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", reservation.ClientId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", reservation.EventId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventId,NumberOfGuests")] Reservation reservation)
        {

            if (id != reservation.Id)
            {
                return NotFound();
            }

            reservation.ReservationDate = DateTime.Now;
            reservation.ClientId = _userManager.GetUserId(User)!;
            ClearServerManagedReservationValidation();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Reservations.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Reservation updated successfully";
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", reservation.ClientId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", reservation.EventId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Reservation deleted successfully";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }

        private void ClearServerManagedReservationValidation()
        {
            ModelState.Remove(nameof(Reservation.ClientId));
            ModelState.Remove(nameof(Reservation.Client));
            ModelState.Remove(nameof(Reservation.Event));
            ModelState.Remove(nameof(Reservation.ReservationDate));
        }
    }
}
