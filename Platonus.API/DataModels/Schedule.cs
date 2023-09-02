using System;
using System.Collections.Generic;
using System.Globalization;

namespace Platonus.API.DataModels;

public class Schedule
{
    public int SelectedYear { get; }
    public int SelectedTermNumber { get; }
    public int SelectedWeekNumber { get; }
    public int CurrentWeekNumber { get; }
    public List<int> TermWeekNumbers { get; }
    
    internal Schedule(ScheduleResponse scheduleData)
    {
        SelectedYear = scheduleData.selectedStudyYear;
        SelectedTermNumber = scheduleData.selectedTerm;
        SelectedWeekNumber = scheduleData.selectedWeek;
        CurrentWeekNumber = scheduleData.activeWeekNumber;
        TermWeekNumbers = scheduleData.weekList;

        Days = new Day[scheduleData.timetable.days.Count];
        for (int di = 0; di < Days.Length; di++)
        {
            var dayData = scheduleData.timetable.days[(di + 1).ToString()];
            Days[di] = new Day(di, dayData.lessons.Count);
            int li = 0;
            foreach (var lessonWrapper in dayData.lessons)
            {
                var lessonHourData = scheduleData.lessonHours.Find(lh => lh.number.ToString() == lessonWrapper.Key);
                if (lessonHourData is null)
                    throw new NullReferenceException($"lesson hour with number {lessonWrapper.Key} not found");
                Days[di].LessonHours[li].Time = new TimeRange(lessonHourData.start, lessonHourData.finish);

                switch (lessonWrapper.Value.lessons.Count)
                {
                    case 0:
                        Days[di].LessonHours[li].Lesson = null;
                        break;
                    case 1:
                        var lessonData = lessonWrapper.Value.lessons[0];
                        Days[di].LessonHours[li].Lesson = new Day.Lesson(lessonData);
                        break;
                    default:
                        throw new Exception($"lessonWrapper has data for more than one lessons ({lessonWrapper.Value.lessons.Count})");
                }
                
                li++;
            }
        }
    }

    public readonly struct TimeRange
    {
        public readonly TimeSpan Start;
        public readonly TimeSpan Finish;
        
        public TimeRange(TimeSpan from, TimeSpan to)
        {
            Start = from;
            Finish = to;
        }

        /// format: hh:mm:ss
        public TimeRange(string startStr, string finishStr)
        {
            Start = TimeSpan.ParseExact(startStr, @"hh\:mm\:ss", CultureInfo.InvariantCulture);
            Finish = TimeSpan.ParseExact(finishStr, @"hh\:mm\:ss", CultureInfo.InvariantCulture);
        }
    }
    
    /// days of week (monday=0, sunday=6)
    public Day[] Days { get; }

    public class Day
    {
        public (TimeRange Time, Lesson? Lesson)[] LessonHours { get; }
        public DayOfWeek DayOfWeek { get; }
        internal Day(int dayIndex, int dayLessonsCount)
        {
            LessonHours = new (TimeRange, Lesson?)[dayLessonsCount];
            DayOfWeek = dayIndex switch
            {
                0 => DayOfWeek.Monday,
                1 => DayOfWeek.Tuesday,
                2 => DayOfWeek.Wednesday,
                3 => DayOfWeek.Thursday,
                4 => DayOfWeek.Friday,
                5 => DayOfWeek.Saturday,
                6 => DayOfWeek.Sunday,
                _ => throw new Exception($"there is no day of week with index {dayIndex}")
            };
        }

        public class Lesson
        {
            // example: Жанабеков Ж.О
            public string TutorName { get; }
            /// example: 420 б
            public string Auditory { get; }
            /// example: Байзак центр
            public string Building { get; }
            
            /// example: Lecture
            public string LessonTypeName { get; }
            /// example: Программирование на языке С#
            public string SubjectName { get; }
            /// may not work, idk
            public bool OnlineClass { get; }

            internal Lesson(ScheduleResponse.Timetable.Day.LessonWrapper.Lesson lessonData)
            {
                TutorName = lessonData.tutorName;
                Auditory = lessonData.auditory;
                Building = lessonData.building;
                LessonTypeName = lessonData.groupTypeFullName;
                SubjectName = lessonData.subjectName;
                OnlineClass = lessonData.onlineClass;
            }
        }
    }
}