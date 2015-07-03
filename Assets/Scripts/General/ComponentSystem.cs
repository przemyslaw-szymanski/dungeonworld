using System.Collections.Generic;

//T component type
//C base component type
public class ComponentSystem<T, C> where C: class, new() where T : C
{
    protected List<T> m_Components = new List<T>();

}
