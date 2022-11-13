﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamerForumWeb.Db.Migrations
{
    public partial class fixingVoteEntity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_PostsComments_CommentId",
                table: "Votes");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Votes",
                newName: "PostCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_CommentId",
                table: "Votes",
                newName: "IX_Votes_PostCommentId");

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 11, 13, 13, 36, 41, 360, DateTimeKind.Local).AddTicks(6177));

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2022, 11, 13, 13, 36, 41, 360, DateTimeKind.Local).AddTicks(6230));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 11, 13, 13, 36, 41, 360, DateTimeKind.Local).AddTicks(6273));

            migrationBuilder.UpdateData(
                table: "PostsComments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 11, 13, 13, 36, 41, 360, DateTimeKind.Local).AddTicks(6319));

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_PostsComments_PostCommentId",
                table: "Votes",
                column: "PostCommentId",
                principalTable: "PostsComments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_PostsComments_PostCommentId",
                table: "Votes");

            migrationBuilder.RenameColumn(
                name: "PostCommentId",
                table: "Votes",
                newName: "CommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_PostCommentId",
                table: "Votes",
                newName: "IX_Votes_CommentId");

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 11, 13, 12, 47, 48, 644, DateTimeKind.Local).AddTicks(8133));

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2022, 11, 13, 12, 47, 48, 644, DateTimeKind.Local).AddTicks(8177));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 11, 13, 12, 47, 48, 644, DateTimeKind.Local).AddTicks(8223));

            migrationBuilder.UpdateData(
                table: "PostsComments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 11, 13, 12, 47, 48, 644, DateTimeKind.Local).AddTicks(8273));

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_PostsComments_CommentId",
                table: "Votes",
                column: "CommentId",
                principalTable: "PostsComments",
                principalColumn: "Id");
        }
    }
}
