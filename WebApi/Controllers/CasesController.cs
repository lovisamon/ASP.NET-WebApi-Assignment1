using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CasesController : ControllerBase
    {
        private readonly SqlDbContext _context;

        public CasesController(SqlDbContext context)
        {
            _context = context;
        }

        #region CUSTOM REQUESTS

        // GET: api/Cases/Client
        [HttpGet("Client/{client}")]
        public async Task<ActionResult<IEnumerable<Case>>> GetCasesByClient(string client)
        {
            var @case = await _context.Cases.Where(x => x.Client == client).ToListAsync();

            if (@case == null || !@case.Any())
            {
                return NotFound();
            }

            return @case;
        }

        // GET: api/Cases/CreatedAfter
        [HttpGet("CreatedAfter/{inputDatetime}")]
        public async Task<ActionResult<IEnumerable<Case>>> GetCasesByDatetime(string inputDatetime)
        {
            if (DateTime.TryParse(inputDatetime, out var datetime))
            {
                return await _context.Cases.Where(x => x.Created >= datetime).ToListAsync();
            }

            return BadRequest();
        }

        // GET: api/Cases/CurrentStatus
        [HttpGet("CurrentStatus/{currentStatus}")]
        public async Task<ActionResult<IEnumerable<Case>>> GetCasesByCurrentStatus(string currentStatus)
        {
            var @case = await _context.Cases.Where(x => x.CurrentStatus == currentStatus).ToListAsync();

            if (@case == null || !@case.Any())
            {
                return NotFound();
            }

            return @case;
        }

        // Only possible to update the current status of a case.
        // PATCH: api/Cases/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCase(int id, CaseUpdate @case)
        {
            var fetchedCase = await _context.Cases.FindAsync(id);

            if (fetchedCase == null)
            {
                return NotFound();
            }

            _context.Entry(fetchedCase).State = EntityState.Modified;
            fetchedCase.Edited = DateTime.Now;
            fetchedCase.CurrentStatus = @case.CurrentStatus;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CaseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        #endregion

        #region TEMPLATE REQUESTS

        // GET: api/Cases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Case>>> GetCases()
        {
            return await _context.Cases.ToListAsync();
        }

        // GET: api/Cases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Case>> GetCase(int id)
        {
            var @case = await _context.Cases.FindAsync(id);

            if (@case == null)
            {
                return NotFound();
            }

            return @case;
        }

        // PUT: api/Cases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCase(int id, Case @case)
        //{
        //    if (id != @case.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(@case).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CaseExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Cases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Case>> PostCase(Case @case)
        {
            @case.Created = DateTime.Now;
            @case.Edited = null;    // Do not allow adding an edited date on creation.
            _context.Cases.Add(@case);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCase", new { id = @case.Id }, @case);
        }

        // DELETE: api/Cases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCase(int id)
        {
            var @case = await _context.Cases.FindAsync(id);
            if (@case == null)
            {
                return NotFound();
            }

            _context.Cases.Remove(@case);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CaseExists(int id)
        {
            return _context.Cases.Any(e => e.Id == id);
        }

        #endregion
    }
}
