﻿using Domain.AutoPosting;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace UseCases.AutoPosts
{
    public class AutoPostCondition
    {
        public bool CheckAutoPost(AutoPostCache cache, ref string message)
        {
            if (CheckFiles(cache.files, ref message))
            {
                if (cache.post_type)
                {
                    return CheckPost(cache, ref message);
                }
                else
                {
                    return CheckStories(cache, ref message);
                }
            }
            return false;
        }
        public bool CheckStories(AutoPostCache cache, ref string message)
        {
            if (!CheckExecuteTime(cache.execute_at, cache.timezone, ref message))
            {
                return false;
            }
            if (!CheckDeleteAfter(cache.auto_delete, cache.delete_after, cache.execute_at, ref message))
            {
                return false;
            }
            if (!CheckToUpdateTimezone(cache.timezone, ref message))
            {
                return false;
            }
            if (!CheckCategory(cache.session_id, cache.category_id, ref message))
            {
                return false;
            }
            return true;
        }
        public bool CheckPost(AutoPostCache cache, ref string message)
        {
            if (!CheckStories(cache, ref message))
            {
                return false;
            }
            if (!CheckToUpdateDescription(cache.description, ref message))
            {
                return false;
            }
            if (!CheckToUpdateComment(cache.comment, ref message))
            {
                return false;
            }
            if (!CheckToUpdateLocation(cache.location, cache.session_id, ref message))
            {
                return false;
            }
            return true;
        }
        public bool CheckExecuteTime(DateTime executeAt, int timezone, ref string message)
        {
            timezone = timezone > 0 ? -timezone : timezone * -1;
            if (executeAt.AddHours(timezone) > DateTimeOffset.UtcNow)
            {
                return true;
            }
            message = "Auto post can't be execute in past.";
            return false;
        }
        public bool CheckLocation(string location, long sessionId, ref string message)
        {
            if (!string.IsNullOrEmpty(location))
            {
                var session = AccountManager.LoadSession(sessionId);
                if (session != null)
                {
                    return handler.CheckExistLocation(ref session, location, 0, 0, ref message);
                }
                else
                {
                    message = "Server can't define session.";
                }
            }
            else
            {
                message = "Location can't be null or empty.";
            }
            Logger.Warning(message);
            return false;
        }
        public bool CheckDescription(string description, ref string message)
        {
            if (!string.IsNullOrEmpty(description))
            {
                if (description.Length < 2200)
                {
                    if (UploadedTextIsTrue(description, ref message))
                    {
                        return true;
                    }
                }
                else
                {
                    message = "Description can't be more 2200 characters.";
                }
            }
            else
            {
                message = "Description is null or empty.";
            }
            Logger.Warning(message);
            return false;
        }
        public bool CheckComment(string comment, ref string message)
        {
            if (!string.IsNullOrEmpty(comment))
            {
                if (comment.Length < 256)
                {
                    if (UploadedTextIsTrue(comment, ref message))
                    {
                        return true;
                    }
                }
                else
                {
                    message = "Comment can't be more 256 characters.";
                }
            }
            else
            {
                message = "comment is null or empty.";
            }
            Logger.Warning(message);
            return false;
        }
        public bool CheckFiles(ICollection<IFormFile> files, ref string message)
        {
            if (files != null)
            {
                if (files.Count > 0 && files.Count <= 10)
                {
                    foreach (IFormFile file in files)
                    {
                        if (!CheckFileType(file.ContentType, ref message))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    message = "Required count of files from 1 to 10.";
                }
            }
            else
            {
                message = "No post files here.";
            }
            Logger.Warning(message);
            return false;
        }
        public bool CheckFileType(string contentType, ref string message)
        {
            if (contentType.Contains("video"))
            {
                return true;
            }
            if (contentType.Contains("image") || contentType.Contains("application/octet-stream"))
            {
                return true;
            }
            message = "File has incorrect format. Required format -> 'image' or 'video'.";
            return false;
        }
        public bool CheckDeleteAfter(bool? autoDelete, DateTime deleteAfter, DateTime executeAt, ref string message)
        {
            if (autoDelete != null)
            {
                if (autoDelete == false)
                {
                    Logger.Information("Auto delete option is off.");
                    return true;
                }
                else
                {
                    if (deleteAfter > executeAt)
                    {
                        return true;
                    }
                    else
                    {
                        message = "Delete after option can't be less that execute at.";
                    }
                }
            }
            else
            {
                message = "Option 'auto_delete' is null.";
            }
            return false;
        }
        public bool CheckCategory(long accountId, long categoryId, ref string message)
        {
            if (categoryId != 0)
            {
                var category = CategoryRepository.GetBy(accountId, categoryId, false);
                if (category != null)
                {
                    return true;
                }
                message = "Server can't define category by id.";
                return false;
            }
            return true;
        }
        public bool CheckToUpdateStories(AutoPostCache cache, ref string message)
        {
            if (!CheckToUpdateExecuteTime(cache.execute_at, cache.timezone, ref message))
            {
                return false;
            }
            if (!CheckDeleteAfter(cache.auto_delete, cache.delete_after, cache.execute_at, ref message))
            {
                return false;
            }
            return true;
        }
        public bool CheckToUpdatePost(AutoPostCache cache, ref string message)
        {
            if (!CheckToUpdateStories(cache, ref message))
            {
                return false;
            }
            if (!CheckToUpdateDescription(cache.description, ref message))
            {
                return false;
            }
            if (!CheckToUpdateComment(cache.comment, ref message))
            {
                return false;
            }
            if (!CheckToUpdateLocation(cache.location, cache.session_id, ref message))
            {
                return false;
            }
            if (!CheckToUpdateTimezone(cache.timezone, ref message))
            {
                return false;
            }
            if (!CheckCategory(cache.session_id, cache.category_id, ref message))
            {
                return false;
            }
            return true;
        }
        public bool CheckToUpdateExecuteTime(DateTime executeTime, int timezone, ref string message)
        {
            if (executeTime != null)
            {
                return CheckExecuteTime(executeTime, timezone, ref message);
            }
            return true;
        }
        public bool CheckToUpdateDeleteAfter(bool? autoDelete, DateTime deleteAfter, DateTime execute_at, int timezone, ref string message)
        {
            if (autoDelete != null && autoDelete == true)
            {
                return CheckDeleteAfter(autoDelete, deleteAfter, execute_at, ref message);
            }
            return true;
        }
        public bool CheckToUpdateLocation(string location, long sessionId, ref string message)
        {
            if (!string.IsNullOrEmpty(location))
            {
                return CheckLocation(location, sessionId, ref message);
            }
            return true;
        }
        public bool CheckToUpdateDescription(string description, ref string message)
        {
            if (!string.IsNullOrEmpty(description))
            {
                return CheckDescription(description, ref message);
            }
            return true;
        }
        public bool CheckToUpdateComment(string comment, ref string message)
        {
            if (!string.IsNullOrEmpty(comment))
            {
                return CheckComment(comment, ref message);
            }
            return true;
        }
        public bool CheckToUpdateTimezone(int timezone, ref string message)
        {
            if (timezone != 0)
            {
                if (timezone > -14 && timezone < 14)
                {
                    return true;
                }
                message = "Timezone can't be less -14 & more 14 hours";
                return false;
            }
            return true;
        }
        public bool HashtagsIsTrue(string uploadedText, ref string message)
        {
            int hashtagsCount = 0; bool result = false;

            hashtagsCount = Regex.Matches(uploadedText, @"#(\w+)").Cast<Match>().ToArray().Length;
            result = hashtagsCount <= availableHashtags;
            if (!result)
            {
                message = "Hashtags can't be more than " + availableHashtags + ".";
            }
            return result;
        }
        public bool TagsIsTrue(string uploadedText, ref string message)
        {
            int tagsCount = 0; bool result = false;

            tagsCount = Regex.Matches(uploadedText, @"@(\w+)").Cast<Match>().ToArray().Length;
            result = tagsCount <= availableTags;
            if (!result)
            {
                message = "Tags can't be more than " + availableTags + ".";
            }
            return result;
        }
    }
}