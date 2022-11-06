﻿using GamerForumWeb.Db.Data.Entities;
using System.ComponentModel.DataAnnotations;
using static GamerForumWeb.Db.Data.Common.DataValidationConstants.Game;

namespace GamerForumWeb.Core.Models.Game
{
    public class GameModel
    {
        [Required]
        [StringLength(MaxTitleLenght)]
        public string Title { get; set; }

        [Required]
        [StringLength(MaxStudioLenght)]
        public string Studio { get; set; }

        [Required]
        [MaxLength(MaxDescriptionLenght)]
        public string Description { get; set; }

        [Required]
        public double Rating { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
