public class CheckNameSpaceExist
{
    public static void CheckNameSpaceUserExist(string namespaceName)
    {

        if (namespaceName.Contains("FirstProject.Domain.Entities.Group"))
        {
            Console.WriteLine("Group tồn tại");
        }

        else
        {
            Console.WriteLine("Không tồn tại");
        }

    }
}