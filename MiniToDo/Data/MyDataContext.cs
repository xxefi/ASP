using ToDo.Models;

namespace ToDo.Data;

public class MyDataContext
{
    public List<PostModel> Posts { get; set; }
    public MyDataContext() => Posts = [];
}
