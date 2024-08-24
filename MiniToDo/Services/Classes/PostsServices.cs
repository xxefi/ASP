using Microsoft.Extensions.Hosting;
using ToDo.Data;
using ToDo.Models;
using ToDo.Services.Interfaces;

namespace ToDo.Services.Classes;

public class PostsServices : IPostServices
{
    private readonly MyDataContext _dataContext;

    public PostsServices(MyDataContext dataContext) => _dataContext = dataContext;
    public PostModel Create(PostModel post)
    {
        var lastPost = _dataContext.Posts.LastOrDefault();
        int newId = lastPost is null ? 1 : lastPost.Id + 1;

        post.Id = newId;
        _dataContext.Posts.Add(post);
        return post;
    }

    public void Delete(int id)
    {
        var modelToDelete = _dataContext.Posts.FirstOrDefault(x => x.Id == id)
            ?? throw new Exception($"Post with Id {id} not found");
        _dataContext.Posts.Remove(modelToDelete);
    }

    public PostModel Get(int id)
    {
        return _dataContext.Posts.FirstOrDefault(x => x.Id == id)
            ?? throw new Exception($"Post with Id {id} not found");
    }

    public List<PostModel> GetAll()
    {
        return _dataContext.Posts
            ?? throw new Exception("Posts is null");
    }

    public PostModel Update(PostModel post)
    {
        var modelToUpdate = _dataContext.Posts.FirstOrDefault(x => x.Id == post.Id) 
            ?? throw new Exception($"Post with Id {post.Id} not found");
        modelToUpdate.Header = post.Header;
        modelToUpdate.Text = post.Text;

        return modelToUpdate;
    }
}
