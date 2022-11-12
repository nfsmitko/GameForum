﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamerForumWeb.Db.Data.Entities
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }

        public int PostCommentId { get; set; }

        [ForeignKey(nameof(PostCommentId))]
        public virtual PostComment Comment { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        public VoteType Type { get; set; }

    }
}
