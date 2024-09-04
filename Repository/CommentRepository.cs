using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBcontext _context;
        private readonly IStockRepository _StockRepo;
        public CommentRepository(ApplicationDBcontext context, IStockRepository StockRepo)
        {
            _context = context;
            _StockRepo = StockRepo;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(a => a.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var comment = await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
            return comment;
        }

        public async Task<Comment> CreateAsync(Comment commentDto)
        {
            await _context.Comments.AddAsync(commentDto);
            await _context.SaveChangesAsync();
            return commentDto;
        }

        public async Task<Comment?> UpdateAsync(int commentId, CreateCommentDto commentDto)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
                return null;
            comment.Title = commentDto.Title;
            comment.Content = commentDto.Content;

            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
                return null;
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}