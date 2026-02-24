using Library_systemEF.Model;

namespace Library_systemEF.Repositories;

public interface IMemberRepositorie
{

  Task AddAsync(Member member);
  

}