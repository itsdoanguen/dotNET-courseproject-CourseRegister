﻿using System.ComponentModel.DataAnnotations;

namespace dotNET_courseproject_CourseRegister.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required, StringLength(100)]
        public string CourseName { get; set; }

        [Required, StringLength(100)]
        public string TeacherName { get; set; }
        [Required]
        public string CourseDescription { get; set; }
        [Required]
        public string CourseImage { get; set; } = "default.jpg";
        [Required]
        public DateTime StartedTime { get; set; }

        [Required]
        public double Cost { get; set; }

        [Required]
        public int MaxStudents { get; set; }
        [Required]
        public int CurrentStudents { get; set; }

        [Required]
        public int Duration { get; set; }
        [Required]
        public CourseStatus Status { get; set; } = CourseStatus.Active;

        public ICollection<User_Course>? EnrolledUsers { get; set; }

        public enum CourseStatus
        {
            Active,
            Inactive
        }
    }
}
