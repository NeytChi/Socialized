﻿using UseCases.AutoPosts.AutoPostFiles.Commands;

namespace UseCases.AutoPosts.Commands
{
    public class UpdateAutoPostCommand : AutoPostCommand
    {
        public required string UserToken { get; set; }
        public long PostId { get; set; }
        public ICollection<UpdateAutoPostFileCommand>? Files { get; set; }
    }
}
