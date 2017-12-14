using System;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GOAPExample : MonoBehaviour
{
    public int desiredSkill = 4;
    public Student initialState;

    [Serializable]
    public class Student
    {
        public int skill;
        public int energy;
        public int time;

        public Student Clone()
        {
            return new Student()
            {
                skill = this.skill,
                energy = this.energy,
                time = this.time
            };
        }
    }

    void Start()
    {
        Func<Student, bool> condition = student => student.skill >= desiredSkill;
        Func<Student, float> heuristic = student => desiredSkill - student.skill;
        Func<Student, Student> clone = student => student.Clone();

        var study = new GOAP.Action<string, Student>
            (
            "Study",
            student => student.energy >= 1 && student.time >= 1,
            student =>
            {
                student.skill++;
                student.energy--;
                student.time--;
                return Transition.Create(student, 1f);
            }
            );

        var sleep = new GOAP.Action<string, Student>
            (
            "Sleep",
            student => student.time >= 1,
            student =>
            {
                student.energy++;
                student.time--;
                return Transition.Create(student, 1f);
            }
            );        

        var actions = new List<GOAP.Action<string, Student>>() { study, sleep };

        var plan = Planner<Student, GOAP.Action<string, Student>>
            .Plan(initialState, actions, condition, heuristic, clone);

        if (plan != null)
        {
            foreach (var item in plan)
            {
                Debug.Log(item.userData);
            }
        }
        else
        {
            Debug.Log("Can't find a suitable plan.");
        }

    }

}


