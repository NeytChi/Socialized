﻿using Microsoft.AspNetCore.Http;

namespace UseCases.AutoPosts.Commands
{
    public class CreateAutoPostCommand
    {
        public string UserToken { get; set; }
        public long AccountId { get; set; }

        public bool AutoPostType { get; set; }
        public List<IFormFile> Files { get; set; }
        public List<long> FileIds { get; set; }
        public DateTime ExecuteAt { get; set; }
        public bool? AutoDelete { get; set; }
        public DateTime DeleteAfter { get; set; }
        public string Location {  get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public long CategoryId { get; set; }
        public int Next { get; set; }
        public int Count { get; set; }
        public DateTime From { get; set; }
        public DateTime to;
        public long file_id;
        public sbyte order;
        public int TimeZone { get; set; }
    }
}
