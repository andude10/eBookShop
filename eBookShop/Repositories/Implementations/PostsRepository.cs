using eBookShop.Data;
using eBookShop.Models;
using eBookShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eBookShop.Repositories.Implementations;

public class PostsRepository : IPostsRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public PostsRepository(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <summary>
    ///     GetUser returns a post WITHOUT associated data. To load related data, you need to use the LoadList() methods
    /// </summary>
    /// <returns>Post WITHOUT associated data</returns>
    public Post GetPost(int id)
    {
        using var dbContext = _contextFactory.CreateDbContext();
        
        var post = dbContext.Posts.Find(id);

        if (post == null)
        {
            throw new KeyNotFoundException($"No post found with id {id}");
        }
        
        return post;
    }

    public IEnumerable<Post> GetPosts()
    {
        using var dbContext = _contextFactory.CreateDbContext();
        return dbContext.Posts.ToList();
    }

    public void LoadPostAuthor(Post post)
    {
        using var dbContext = _contextFactory.CreateDbContext();
        
        dbContext.Entry(post).State = EntityState.Unchanged;
        dbContext.Entry(post).Reference(p => p.User).Load();
        dbContext.Entry(post).State = EntityState.Detached;
    }

    public void Create(Post item)
    {
        using var dbContext = _contextFactory.CreateDbContext();
        dbContext.Add(item);
        dbContext.SaveChanges();
    }

    public void Update(Post item)
    {
        using var dbContext = _contextFactory.CreateDbContext();
        dbContext.Update(item);
        dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        using var dbContext = _contextFactory.CreateDbContext();

        var post = dbContext.Posts.Find(id);

        if (post == null)
        {
            throw new KeyNotFoundException($"Post with {id.ToString()} id is Not found");
        }

        dbContext.Posts.Remove(post);
        dbContext.SaveChanges();
    }
}