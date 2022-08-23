using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class DoubleLinkedLists<T>: IEnumerable<T>
    {
        Node first = null;
        Node last = null;

        public override string ToString()
        {
            StringBuilder s = new StringBuilder(); //new empty string list
            Node temp = first; //new temp looking first at start
            while (temp != null) //untill current index isnt null or list isnt empty:
            {
                s.Append($"{temp.value} "); //convert the current object to string & add it to the s String list
                temp = temp.next; //temp is next index
            }
            return s.ToString();
        }
        public string GetAt(object value)
        {
            StringBuilder s = new StringBuilder(); //new empty string list
            Node temp = first; //new temp looking first at start
            int index = -1; //reset index to start from -1 so first index will be 0 & if there isnt a match will return nothing
            int count = index; //set a counter to -1 to check if there wasent any match

            while (temp != null) //untill current index isnt null or list isnt empty:
            {
                index++; //index for current node to print when there is a match (starts from zero)
                if (temp.value.Equals(value)) //if the Node value == to inserted value
                {
                    s.Append($"{index} , "); //convert the current object to string & add it to the s String list
                    count++; //make sure method knows there was atleast one match
                }
                temp = temp.next; //advence in Node
            }

            if (count == -1) return count.ToString(); //if there werent any matches -> print "-1"
            return s.ToString(); //print all matches indexes
        }
        public void AddFirst(T value)
        {
            Node n = new Node(value); //create a new data container (looking next at null)
            if (first == null) last = n; //if the list is empty the first is also last

            n.next = first; //the new data next look will look at current start
            first = n; //the new index is the new start of list
            //new start prev in null auto by struct
        }
        public void AddLast(T value)
        {
            if (first == null) //if the list is empty:
            {
                AddFirst(value); //add the wantwd value to the list
                return;
            }

            Node n = new Node(value); //n is a new data container
            n.prev = last; //current last becomes new index previous
            last.next = n; //cuerrent last next is looking at new index
            last = n; //known last index is set to the new index
        }
        //public void RemoveFirst() //
        //{
        //    if (first == null) return; //if the list is empty do nothing
        //    first = first.next; //else -> advence list start to the next object in list
        //    first.prev = null; //new first priviuos is looking at null

        //    if (first == null) last = null; //if after deleting: all the list is clean then last is also null
        //}
        public bool RemoveFirst() //remove first & return the value of removed object
        {
            if (first == null) return false; //if the list is empty return false (cant delete)

            first = first.next; //advence list start to the next object in list
            first.prev = null; //new first priviuos is looking at null
            return true; //return true (success at deleting)
        }
        public void RemoveLast()
        {
            if (last == null) return; //if there is no last (meaning there is no first also) do nothing
            last = last.prev; //else -> back list last to the previous object of current last
            last.next = null; //new last next is looking at null

            if (last == null) first = null; //if after deleting: all the list is clean then first is also null
        }
        public bool RemoveLast(out T removedValue) //remove last & return the value of removed object
        {
            removedValue = default; //auto return 0 for value of object removed
            if (last == null) return false; //if the list is empty return false (cant delete)

            removedValue = last.value; //set the returned value to be the value of removed object
            last = last.prev; //back list last to the previous object of current last
            last.next = null; //new last next is looking at null
            return true; //return true (success at deleting)
        }
        public IEnumerator<T> GetEnumerator()
        {
            Node current = first;
            while (current != null)
            {
                yield return current.value;
                current = current.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {

            return this.GetEnumerator();
        }

        class Node
        {
            //public fields (known only to private LinkedList Class):
            public T value; //object at current index
            public Node next; //looking at next obj location
            public Node prev; //looking at previous location

            //construct:
            public Node(T value) //asking for an object
            {
                this.value = value; //current instanse value is inserted value
                next = null; //when creating new list next is null becuse it isnt existing yet (untill adding it)
                prev = null; //when creating new list prev is null becuse it isnt existing yet (untill going forward)
            }
        }
    }
}
