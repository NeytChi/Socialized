using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DefaultAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppealReplies_AppealMessages_AppealMessageId",
                table: "AppealReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoPosts_IGAccounts_AccountId",
                table: "AutoPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessAccounts_IGAccounts_AccountId",
                table: "BusinessAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_IGAccounts_AccountId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_IGAccounts_Users_UserId",
                table: "IGAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionProfiles_IGAccounts_AccountId",
                table: "SessionProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_States_IGAccounts_AccountId",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_States_timeAction_TimeActionId",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskGS_IGAccounts_AccountId",
                table: "TaskGS");

            migrationBuilder.DropForeignKey(
                name: "FK_timeAction_IGAccounts_AccountId",
                table: "timeAction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_Users_UserId",
                table: "UserProfile");

            migrationBuilder.DropTable(
                name: "CommentStatistics");

            migrationBuilder.DropTable(
                name: "FilterWords");

            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropTable(
                name: "OnlineFollowers");

            migrationBuilder.DropTable(
                name: "Statistics");

            migrationBuilder.DropTable(
                name: "StoryStatistics");

            migrationBuilder.DropTable(
                name: "PostStatistics");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_timeAction",
                table: "timeAction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IGAccounts",
                table: "IGAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_countries",
                table: "countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfile",
                table: "UserProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionProfiles",
                table: "SessionProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppealReplies",
                table: "AppealReplies");

            migrationBuilder.RenameTable(
                name: "timeAction",
                newName: "TimeAction");

            migrationBuilder.RenameTable(
                name: "IGAccounts",
                newName: "IgAccounts");

            migrationBuilder.RenameTable(
                name: "countries",
                newName: "Countries");

            migrationBuilder.RenameTable(
                name: "UserProfile",
                newName: "Profiles");

            migrationBuilder.RenameTable(
                name: "SessionProfiles",
                newName: "AccountProfiles");

            migrationBuilder.RenameTable(
                name: "AppealReplies",
                newName: "AppealMessageReplies");

            migrationBuilder.RenameIndex(
                name: "IX_timeAction_AccountId",
                table: "TimeAction",
                newName: "IX_TimeAction_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_IGAccounts_UserId",
                table: "IgAccounts",
                newName: "IX_IgAccounts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfile_UserId",
                table: "Profiles",
                newName: "IX_Profiles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SessionProfiles_AccountId",
                table: "AccountProfiles",
                newName: "IX_AccountProfiles_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_AppealReplies_AppealMessageId",
                table: "AppealMessageReplies",
                newName: "IX_AppealMessageReplies_AppealMessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeAction",
                table: "TimeAction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IgAccounts",
                table: "IgAccounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Profiles",
                table: "Profiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountProfiles",
                table: "AccountProfiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppealMessageReplies",
                table: "AppealMessageReplies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "WordFilters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FilterId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Use = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordFilters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordFilters_TaskFilters_FilterId",
                        column: x => x.FilterId,
                        principalTable: "TaskFilters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Email", "FirstName", "IsDeleted", "LastLoginAt", "LastName", "LastUpdatedAt", "Password", "RecoveryCode", "Role", "TokenForStart" },
                values: new object[] { 1L, new DateTime(2025, 1, 23, 12, 9, 44, 346, DateTimeKind.Local).AddTicks(8834), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user@example.com", "default", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "default", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", null, "default", "" });

            migrationBuilder.CreateIndex(
                name: "IX_WordFilters_FilterId",
                table: "WordFilters",
                column: "FilterId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountProfiles_IgAccounts_AccountId",
                table: "AccountProfiles",
                column: "AccountId",
                principalTable: "IgAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppealMessageReplies_AppealMessages_AppealMessageId",
                table: "AppealMessageReplies",
                column: "AppealMessageId",
                principalTable: "AppealMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoPosts_IgAccounts_AccountId",
                table: "AutoPosts",
                column: "AccountId",
                principalTable: "IgAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessAccounts_IgAccounts_AccountId",
                table: "BusinessAccounts",
                column: "AccountId",
                principalTable: "IgAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_IgAccounts_AccountId",
                table: "Categories",
                column: "AccountId",
                principalTable: "IgAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IgAccounts_Users_UserId",
                table: "IgAccounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Users_UserId",
                table: "Profiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_States_IgAccounts_AccountId",
                table: "States",
                column: "AccountId",
                principalTable: "IgAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_States_TimeAction_TimeActionId",
                table: "States",
                column: "TimeActionId",
                principalTable: "TimeAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskGS_IgAccounts_AccountId",
                table: "TaskGS",
                column: "AccountId",
                principalTable: "IgAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeAction_IgAccounts_AccountId",
                table: "TimeAction",
                column: "AccountId",
                principalTable: "IgAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountProfiles_IgAccounts_AccountId",
                table: "AccountProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_AppealMessageReplies_AppealMessages_AppealMessageId",
                table: "AppealMessageReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoPosts_IgAccounts_AccountId",
                table: "AutoPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessAccounts_IgAccounts_AccountId",
                table: "BusinessAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_IgAccounts_AccountId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_IgAccounts_Users_UserId",
                table: "IgAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Users_UserId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_States_IgAccounts_AccountId",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_States_TimeAction_TimeActionId",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskGS_IgAccounts_AccountId",
                table: "TaskGS");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeAction_IgAccounts_AccountId",
                table: "TimeAction");

            migrationBuilder.DropTable(
                name: "WordFilters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeAction",
                table: "TimeAction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IgAccounts",
                table: "IgAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Profiles",
                table: "Profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppealMessageReplies",
                table: "AppealMessageReplies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountProfiles",
                table: "AccountProfiles");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.RenameTable(
                name: "TimeAction",
                newName: "timeAction");

            migrationBuilder.RenameTable(
                name: "IgAccounts",
                newName: "IGAccounts");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "countries");

            migrationBuilder.RenameTable(
                name: "Profiles",
                newName: "UserProfile");

            migrationBuilder.RenameTable(
                name: "AppealMessageReplies",
                newName: "AppealReplies");

            migrationBuilder.RenameTable(
                name: "AccountProfiles",
                newName: "SessionProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_TimeAction_AccountId",
                table: "timeAction",
                newName: "IX_timeAction_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_IgAccounts_UserId",
                table: "IGAccounts",
                newName: "IX_IGAccounts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_UserId",
                table: "UserProfile",
                newName: "IX_UserProfile_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppealMessageReplies_AppealMessageId",
                table: "AppealReplies",
                newName: "IX_AppealReplies_AppealMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountProfiles_AccountId",
                table: "SessionProfiles",
                newName: "IX_SessionProfiles_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_timeAction",
                table: "timeAction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IGAccounts",
                table: "IGAccounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_countries",
                table: "countries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfile",
                table: "UserProfile",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppealReplies",
                table: "AppealReplies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionProfiles",
                table: "SessionProfiles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FilterWords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FilterId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Use = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilterWords_TaskFilters_FilterId",
                        column: x => x.FilterId,
                        principalTable: "TaskFilters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TaskId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.Id);
                    table.ForeignKey(
                        name: "FK_History_TaskGS_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskGS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OnlineFollowers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Value = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineFollowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlineFollowers_BusinessAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "BusinessAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PostStatistics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    CommentsCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Engagement = table.Column<long>(type: "bigint", nullable: false),
                    IGMediaId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Impressions = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LikeCount = table.Column<long>(type: "bigint", nullable: false),
                    MediaType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reach = table.Column<long>(type: "bigint", nullable: false),
                    Saved = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VideoViews = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostStatistics_BusinessAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "BusinessAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EmailContacts = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FollowerCount = table.Column<int>(type: "int", nullable: false),
                    GetDirectionsClicks = table.Column<int>(type: "int", nullable: false),
                    Impressions = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PhoneCallClicks = table.Column<int>(type: "int", nullable: false),
                    ProfileViews = table.Column<long>(type: "bigint", nullable: false),
                    Reach = table.Column<long>(type: "bigint", nullable: false),
                    TextMessageClicks = table.Column<int>(type: "int", nullable: false),
                    WebsiteClicks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statistics_BusinessAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "BusinessAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StoryStatistics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Exists = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Impressions = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MediaId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reach = table.Column<long>(type: "bigint", nullable: false),
                    Replies = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryStatistics_BusinessAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "BusinessAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataId = table.Column<long>(type: "bigint", nullable: false),
                    CommentPk = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    HandleAgain = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HandledAt = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UnitHandled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserIsPrivate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserPk = table.Column<long>(type: "bigint", nullable: false),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Units_TaskData_DataId",
                        column: x => x.DataId,
                        principalTable: "TaskData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CommentStatistics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    InstagramId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MediaId = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentStatistics_PostStatistics_PostId",
                        column: x => x.PostId,
                        principalTable: "PostStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Handled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HandledAt = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Pk = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Queue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medias_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CommentStatistics_PostId",
                table: "CommentStatistics",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_FilterWords_FilterId",
                table: "FilterWords",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_History_TaskId",
                table: "History",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_UnitId",
                table: "Medias",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineFollowers_AccountId",
                table: "OnlineFollowers",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PostStatistics_AccountId",
                table: "PostStatistics",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_AccountId",
                table: "Statistics",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryStatistics_AccountId",
                table: "StoryStatistics",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_DataId",
                table: "Units",
                column: "DataId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppealReplies_AppealMessages_AppealMessageId",
                table: "AppealReplies",
                column: "AppealMessageId",
                principalTable: "AppealMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoPosts_IGAccounts_AccountId",
                table: "AutoPosts",
                column: "AccountId",
                principalTable: "IGAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessAccounts_IGAccounts_AccountId",
                table: "BusinessAccounts",
                column: "AccountId",
                principalTable: "IGAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_IGAccounts_AccountId",
                table: "Categories",
                column: "AccountId",
                principalTable: "IGAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IGAccounts_Users_UserId",
                table: "IGAccounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionProfiles_IGAccounts_AccountId",
                table: "SessionProfiles",
                column: "AccountId",
                principalTable: "IGAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_States_IGAccounts_AccountId",
                table: "States",
                column: "AccountId",
                principalTable: "IGAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_States_timeAction_TimeActionId",
                table: "States",
                column: "TimeActionId",
                principalTable: "timeAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskGS_IGAccounts_AccountId",
                table: "TaskGS",
                column: "AccountId",
                principalTable: "IGAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_timeAction_IGAccounts_AccountId",
                table: "timeAction",
                column: "AccountId",
                principalTable: "IGAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_Users_UserId",
                table: "UserProfile",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
