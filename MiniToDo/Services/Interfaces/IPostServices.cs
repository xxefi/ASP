using ToDo.Models;

namespace ToDo.Services.Interfaces;

public interface IPostServices
{
    PostModel Create(PostModel postDto);
    PostModel Update(PostModel postDto);
    PostModel Get(int id);
    List<PostModel> GetAll();
    void Delete(int id);
}
