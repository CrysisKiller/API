using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        // private readonly ApplicationDBcontext _context;
        private readonly IStockRepository _StockRepo;
        public StockController(/*ApplicationDBcontext _context,*/IStockRepository StockRepo)
        {
            // _context = context;
            _StockRepo = StockRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            var stocks = await _StockRepo.GetAllAsync(query);
            var stockDto = stocks.Select(s => s.ToStockDto()).ToList();
            if (stockDto == null)
                return NotFound();
            return Ok(stockDto);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _StockRepo.GetByIDAsync(id);
            if (stock == null)
                return NotFound();
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stock = await _StockRepo.CreateAsync(stockDto.ToRequestDto());

            return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]

        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _StockRepo.UpdateAsync(id, updateDto);

            if (stockModel == null)
                return NotFound();

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete("{id:int}")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await _StockRepo.DeteteAsync(id);

            if (stock == null)
                return NotFound("Sorry didnt find the record You are trying to delete ");


            return NoContent();
        }

    }
}