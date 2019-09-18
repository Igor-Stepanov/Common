using System;
using System.Collections.Generic;
using System.Linq;
using MessagePack;

namespace GantFormula
{
  [MessagePackObject]
  public abstract class Worker
  {
    [Key(0)] public int Id;
    [Key(1)] public string Task;
    [Key(2)] public List<WorkDay> WorkDays;

    protected Worker() { }
    protected Worker(int id)
    {
      Id = id;
      WorkDays = new List<WorkDay>();
    }

    public void Assign(JiraTask task)
    {
      Task = task.Name;
      task.Assignee = Id;
    }

    public void Work(int day, Dictionary<string, JiraTask> tasks)
    {
      if (Task != null)
      {
        WorkDays.Add(new WorkDay { Task = Task, Day = day});
        
        var task = tasks[Task];
        if (Work(task))
        {
          Task = null;
          task.Assignee = null;
        }
      }
    }

    protected abstract bool Work(JiraTask task);
  }
}