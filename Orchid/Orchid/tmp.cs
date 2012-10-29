public class Parent()
{ 
    public virtual void Draw()
    {
        this.Draw();
    }
}

public class Child1()
{ 
    public override void Draw()
    {
        this.Draw();
    }
}

public class Child2()
{ 
    public override void Draw()
    {
        this.Draw();
    }
}

/////////////////////////

List<Parent> parent_list = new List<Parent>();
parent_list.Add(new Child1());
parent_list.Add(new Child2());

/////////////////////////

foreach (Parent parent in parent_list)
{
    parent.Draw(); //Would like to use child1 and child2's draw
}
