using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Platonus.API.DataModels;

#nullable disable
internal class ScheduleResponse
{
    /// example: 2023
    [JsonRequired] public int selectedStudyYear { get; set; }
    
    /// 1 or 2
    [JsonRequired] public int selectedTerm { get; set; }
    [JsonRequired] public int defaultSelectedTerm { get; set; }
    
    /// selected week number
    [JsonRequired] public int selectedWeek { get; set; }
    [JsonRequired] public int defaultSelectedWeek { get; set; }
    /// current week number
    [JsonRequired] public int activeWeekNumber { get; set; }
    /// all week numbers
    [JsonRequired] public List<int> weekList { get; set; }
    
    /// all lesson hours in the week
    [JsonRequired] public List<LessonHour> lessonHours { get; set; }
    public class LessonHour
    {
        /// number of the lesson in Timetable.Day.lessons
        [JsonRequired] public int number { get; set; }
        /// example: 08:00:00
        [JsonRequired] public string start { get; set; }
        /// example: 08:50:00
        [JsonRequired] public string finish { get; set; }
        /// duration in minutes, usually 50
        [JsonRequired] public int duration { get; set; }
        /// idk what is it
        [JsonRequired] public int shiftNumber { get; set; }
        /// idk what is it
        [JsonRequired] public int displayNumber { get; set; }
    }
    
    /// all days with lesson hours with or without lessons
    [JsonRequired] public Timetable timetable { get; set; }
    public class Timetable
    {
        /// day number string, Day class
        [JsonRequired] public Dictionary<string, Day> days { get; set; }
        public class Day
        {
            /// Lesson.number string, LessonWrapper class
            [JsonRequired] public Dictionary<string, LessonWrapper> lessons { get; set; }
            public class LessonWrapper
            {
                /// one or zero lessons at time
                [JsonRequired] public List<Lesson> lessons { get; set; }
                public class Lesson
                {
                    /// number of the lesson, corresponds a number in a LessonHour 
                    [JsonRequired] public int number { get; set; }
                    /// example: Жанабеков Ж.О
                    [JsonRequired] public string tutorName { get; set; }
                    /// same as tutorName
                    [JsonRequired] public string tutorShortName { get; set; }
                    /// example: 420 б
                    [JsonRequired] public string auditory { get; set; }
                    /// example: Байзак центр
                    [JsonRequired] public string building { get; set; }
                    /// some platonus internal id
                    [JsonRequired] public int lessonID { get; set; }
                    /// some platonus internal group id
                    [JsonRequired] public int studyGroupID { get; set; }
                    /// example: SFT6541-6-L
                    [JsonRequired] public string studygroupShortName { get; set; }
                    /// example: Программирование на языке С#,'Л'
                    [JsonRequired] public string studyGroupName { get; set; }
                    /// example: L
                    [JsonRequired] public string groupTypeShortName { get; set; }
                    /// example: Lecture
                    [JsonRequired] public string groupTypeFullName { get; set; }
                    /// example: Программирование на языке С#
                    [JsonRequired] public string subjectName { get; set; }
                    /// may not work, idk
                    [JsonRequired] public bool onlineClass { get; set; }
                }
            }
        }
    }
}
