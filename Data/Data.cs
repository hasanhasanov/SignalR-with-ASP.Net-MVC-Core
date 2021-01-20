using System;
using System.Collections.Generic;
using System.Linq;
using chat.Data.Entities;

namespace chat.Data
{
    public class Data
    {
        public static class DbInitializer
        {
            public static void Initialize(ChatDbContext context)
            {
                context.Database.EnsureCreated();

                #region Groups         
                if (!context.Groups.Any())
                {
                    var groups = new List<Group>{
                    new Group
                    {
                        Id = 1,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Alfa"
                    },
                    new Group{Id = 2,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Beta"
                    },
                    new Group{Id = 3,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Gamma"
                    }
                };

                    groups.ForEach(x => { context.Groups.Add(x); });
                    context.SaveChanges();
                }

                #endregion

                #region Users

                if (!context.Users.Any())
                {
                    var users = new List<User>{
                    new User
                    {
                        Id = 1,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Hasan"
                    },
                    new User{Id = 2,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Atajan"
                    },
                    new User{Id = 3,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Ahmet"
                    }
                };

                    users.ForEach(x => { context.Users.Add(x); });
                    context.SaveChanges();
                }

                #endregion

                #region Conversations

                if (!context.Conversations.Any())
                {
                    var conversations = new List<Conversation>{
                    new Conversation
                    {
                        Id = 1,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        SenderId = 1,
                        GroupId = 1,
                        Message = "Merhaba millet"
                    },
                    new Conversation
                    {
                        Id = 2,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        SenderId = 2,
                        GroupId = 1,
                        Message = "Selamlar"
                    },
                    new Conversation
                    {
                        Id = 3,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        SenderId = 1,
                        GroupId = 1,
                        Message = "Ahmet neredesin?"
                    },
                    new Conversation
                    {
                        Id = 4,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        SenderId = 3,
                        GroupId = 1,
                        Message = "sa gençler"
                    }
                };

                    conversations.ForEach(x => { context.Conversations.Add(x); });
                    context.SaveChanges();
                }

                #endregion
            }
        }
    }
}