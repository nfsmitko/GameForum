﻿using GamerForumWeb.Core.Contracts;
using GamerForumWeb.Core.Models.Game;
using GamerForumWeb.Db.Data.Entities;
using GamerForumWeb.Db.Repository;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;

namespace GamerForumWeb.Core.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository repo;

        public GameService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task AddNewGame(GameModel model)
        {
            var sanitizor = new HtmlSanitizer();
            var game = new Game()
            {
                Title = sanitizor.Sanitize(model.Title),
                Studio = sanitizor.Sanitize(model.Studio),
                Description = sanitizor.Sanitize(model.Description),
                Rating = model.Rating,
                CreatedDate = DateTime.Now,
                CategoryId = model.CategoryId,
                ImageUrl = sanitizor.Sanitize(model.ImageUrl),
            };
            await repo.AddAsync(game);
            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<GamesQueryModel>> AllGames()
        {
            return await repo.AllReadonly<Game>().OrderByDescending(g=>g.Rating).ThenByDescending(g=>g.Title).Select(g => new GamesQueryModel()
            {
                Id = g.Id,
                ImageUrl = g.ImageUrl,
                Title = g.Title,
                Description = g.Description,
                Rating = g.Rating,
                Studio = g.Studio,
                Category = g.Category.Name

            }).ToListAsync();
        }

        public async Task DeleteGame(int id)
        {
            var game = await repo.All<Game>().Where(g => g.Id == id)
              .Include(p => p.Posts)
              .ThenInclude(c => c.Comments)
              .ThenInclude(v=>v.Votes)
              .FirstOrDefaultAsync();
            if (game == null)
            {
                throw new ArgumentException("Invalid post!");
            }
            foreach (var post in game.Posts)
            {
                foreach (var comment in post.Comments)
                {
                    foreach (var vote in comment.Votes)
                    {
                        await repo.DeleteAsync<Vote>(vote.Id);

                    }
                    await repo.DeleteAsync<PostComment>(comment.Id);
                }
                await repo.DeleteAsync<Post>(post.Id);
            }
            await repo.SaveChangesAsync();
            await repo.DeleteAsync<Game>(id);
            await repo.SaveChangesAsync();
        }

        public async Task<GamesQueryModel> FindeGameByName(string gameName)
        {
            var game = await repo.All<Game>(g => g.Title == gameName).FirstOrDefaultAsync();

            var category = await repo.GetByIdAsync<Category>(game.CategoryId);

            return new GamesQueryModel()
            {
                Id = game.Id,
                ImageUrl = game.ImageUrl,
                Title = game.Title,
                Description = game.Description,
                Rating = game.Rating,
                Studio = game.Studio,
                Category = category.Name
            };
           
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await repo.All<Category>().ToListAsync();
        }

        public async Task<GameModel> GetGameModelById(int gameId)
        {
            var game =  await repo.GetByIdAsync<Game>(gameId);

            if (game == null)
            {
                throw new ArgumentException("Invalid game Id!");
            }

            return new GameModel()
            {
                Title = game.Title,
                Description = game.Description,
                Studio = game.Studio,
                Rating = game.Rating,
                CategoryId = game.CategoryId,
                ImageUrl = game.ImageUrl,
                Categories = repo.All<Category>(),
            };
        }

        public async Task<IEnumerable<GamesQueryModel>> GetGamesByCategory(int categoryId)
        {
            return await repo.All<Game>(g => g.CategoryId == categoryId).Select(g => new GamesQueryModel()
            {
                Id = g.Id,
                Title = g.Title,
                Studio = g.Studio,
                Description = g.Description,
                Rating = g.Rating,
                ImageUrl = g.ImageUrl,
                Category = g.Category.Name
            }).ToListAsync();
        }

        public async Task<IEnumerable<GamesQueryModel>> GetTopGames()
        {
            return await repo.AllReadonly<Game>()
                .OrderByDescending(g => g.Rating)
                .Select(g => new GamesQueryModel()
                {
                    Id = g.Id,
                    ImageUrl = g.ImageUrl,
                    Title = g.Title,
                    Description = g.Description,
                    Rating = g.Rating,
                    Studio = g.Studio,
                    Category = g.Category.Name
                })
                .Take(3)
                .ToListAsync();
        }      

        public async Task UpdateGame(int gameId, GameModel model)
        {
            var game = await repo.GetByIdAsync<Game>(gameId);
            if (game == null)
            {
                throw new ArgumentException("Invalid game ID");
            }
            game.Title = model.Title;
            game.Description = model.Description;
            game.Studio = model.Studio;
            game.Rating = model.Rating;
            game.CategoryId = model.CategoryId; 
            game.ImageUrl = model.ImageUrl;
            game.ModifiedOn = DateTime.Now;

            repo.Update(game);
           await repo.SaveChangesAsync();
        }
    }
}
