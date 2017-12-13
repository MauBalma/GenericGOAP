using System;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GOAPTest : MonoBehaviour
{
    class Student
    {
        public float skill;
        public float energy;
        public float time;

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
        int desiredSkill = 4;

        var initial = new Student { skill = 0, energy = 2, time = 6f };

        Func<Student, bool> condition = student => student.skill >= desiredSkill;
        Func<Student, float> heuristic = student => desiredSkill - student.skill;
        Func<Student, Student> clone = student => student.Clone();

        GenericAction<string, Student> study = new GenericAction<string, Student>
            (
            "Study",
            student => student.energy >= 1 && student.time >= 1,
            student =>
            {
                student.skill++;
                student.energy--;
                student.time--;
                return Tuple.Create(student, 1f);
            }
            );

        GenericAction<string, Student> sleep = new GenericAction<string, Student>
            (
            "Sleep",
            student => student.time >= 1,
            student =>
            {
                student.energy++;
                student.time--;
                return Tuple.Create(student, 1f);
            }
            );        

        var actions = new List<GenericAction<string, Student>>() { study, sleep };

        var plan = Planner<Student, GenericAction<string, Student>>
            .Plan(initial, actions, condition, heuristic, clone);

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


