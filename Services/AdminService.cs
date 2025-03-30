using accoudingWeb.DataBase;
using accoudingWeb.Entities;

namespace accoudingWeb.Services;

public class AdminService(ApplicationContext dbContext)
{
    public bool AddItem<T>(T item) where T : class
    {
        try
        {
            dbContext.Set<T>().Add(item);
            return dbContext.SaveChanges() > 0;
        }
        catch (Exception e)
        {
            
        }
        
        return false;
    }

    public bool DeleteItem<T>(T item) where T : class
    {
        try
        {
            dbContext.Set<T>().Remove(item);
            return dbContext.SaveChanges() > 0;
        }
        catch (Exception e)
        {
            
        }
        
        return false;
    }

    public bool UpdateItem<T>(T item) where T : class
    {
        try
        {
            dbContext.Set<T>().Update(item);
            return dbContext.SaveChanges() > 0;
        }
        catch (Exception e)
        {
            
        }
        
        return false;
    }

    public T? GetItemById<T>(int id) where T : class => dbContext.Set<T>().Find(id);
}