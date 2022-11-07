﻿using GamerForumWeb.Core.Contracts;
using GamerForumWeb.Core.Models.Comment;
using GamerForumWeb.Db.Data;
using GamerForumWeb.Db.Data.Entities;
using GamerForumWeb.Db.Repository;
using Microsoft.EntityFrameworkCore;

namespace GamerForumWeb.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository repo;
        private readonly GamerForumWebDbContext context;

        public CommentService(IRepository _repo, GamerForumWebDbContext _context)
        {
            this.repo = _repo;
            this.context = _context;
        }

        public async Task AddComment(CommentModel model, string userId)
        {
            var post = await context.Posts.FirstOrDefaultAsync(g => g.Id == model.PostId);

            if (post == null)
            {
                throw new ArgumentException("Invalid post ID!");
            }

            var user = await context.Users.Where(u => u.Id == userId).Include(u => u.Posts).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID!");
            }

            var comment = new PostComment()
            {
                Content = model.Content,
                UserId = userId,
                CreatedDate = DateTime.Now,
                PostId = model.PostId,
            };

            post?.Comments.Add(comment);
            user?.Comments.Add(comment);
            await repo.AddAsync(comment);
            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommentQueryModel>> AllComments(int postId)
        {
            return await repo.AllReadonly<PostComment>().Where(c => c.PostId == postId).Select(c => new CommentQueryModel()
            {
                Id = c.Id,
                Content = c.Content,
                PostId = postId,
                CreatedDate = c.CreatedDate,
                UserId = c.UserId,
                Username = c.User.UserName
            }).ToListAsync();
        }

        public async Task DeleteComment(int commentId)
        {
            await repo.DeleteAsync<PostComment>(commentId);
            await repo.SaveChangesAsync();
        }

        public async Task<CommentModel> GetCommentById(int commentId)
        {
            var comment = await repo.GetByIdAsync<PostComment>(commentId);
            if (comment == null)
            {
                throw new ArgumentException("Invalid game Id!");
            }

            return new CommentModel()
            {
                Content = comment.Content,
            };
        }

        public async Task UpdateComment(int commentId, CommentModel model)
        {
            var comment = await repo.GetByIdAsync<PostComment>(commentId);
            if (comment == null)
            {
                throw new ArgumentException("Invalid game ID");
            }
            comment.Content = model.Content;
            comment.UpdatedDate = DateTime.Now;

            repo.Update(comment);
            await repo.SaveChangesAsync();
        }
    }
}
