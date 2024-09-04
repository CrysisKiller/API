using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _StockRepo;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(ICommentRepository commentRepo, IStockRepository StockRepo,UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _StockRepo = StockRepo;
            _userManager=userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comment = await _commentRepo.GetAllAsync();
            var commentDto = comment.Select(comment => comment.ToCommentDto());
            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
                return NotFound();
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{StockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int StockId, [FromBody] CreateCommentDto commentDto)
        {
            if (!await _StockRepo.StockExists(StockId))
                return NotFound("Stock Does not exist");

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var commentModel = commentDto.ToCreateCommentDto(StockId);
            commentModel.AppUserId = appUser.Id;
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut("{CommentId:int}")]

        public async Task<IActionResult> Update([FromRoute] int CommentId, [FromBody] CreateCommentDto commentDto)
        {
            var comment = await _commentRepo.UpdateAsync(CommentId, commentDto);
            if (comment == null)
                return NotFound("Oops the comment ur trying to edit does not exists");
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete("{CommentId:int}")]
         public async Task<IActionResult> Delete([FromRoute] int CommentId){
            var comment = await _commentRepo.DeleteAsync(CommentId);
            if (comment == null)
                return NotFound("Oops the comment ur trying to delete does not exists");

            return NoContent();
        }

    }
}