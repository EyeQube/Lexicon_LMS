
  Lexicon LMS Webbapplication  

  Development team:
* Sattar Alvandpour
* Gunnar Rydberg
* Rolf Bertil Lundqvist

---------------------------------------------------------------------------------------------------------------------------------------

Lexicon LMS Webbapplication is developed and suited for educational organizations, such as schools and academies etc.

Students can access information and get the latest updates about the courses they are attending, such as:

* Viewing their schedule (schedule for each course) 
* Upload and download files/assignments (sharing files with their teacher)
* Read information regarding the modules and activities for the courses they are attending.
* See list of names and email adresses of their classmates/teachers

Administrator/teachers have full access, and permissons for Editing/Adding/Deleting/Updating information: 

* Add/Remove/Edit the schedule with events/activites (each course has their own schedule, and students can only view the schedule, but they can't modify it)
* Register/Remove/Edit students
* Register/Remove/Edit courses
* Add/Remove/Edit modules that belongs to a course
* Add/Remove/Edit activities belonging to a module
* Upload and download files


Step by step instructions for how you run the application (10 steps)
--------------------------------------------------------------------


1. Install DHTMLX Scheduler (*30 day free trial), and Entity Framework.
   Right click on your project in the Solution Explorer and select Manage NuGet Packages. It will open the Manage NuGet Packages dialog box.




2. Now select the Online option in the left bar and search for DHTMLX Scheduler.NET and EntityFramework, and install them both.
 



3. This project needs two configuration files. Each configuration file is supposed to be in their own Migrations folder. One folder (Migrations) already exists, therefore you create only the second migration folder, name it: "Migration". *Note: Second folder does NOT end with letter "s".
   



4. Now you must start enabling  ScheduleContext.cs first, since SchedulerContext.cs inherits from ApplicationDbContext.cs! For that, go to Nuget Package Manager and type:
   Enable-Migrations -ContextTypeName Lexicon_LMS.Models.SchedulerContext   (and press enter)   




5. Add a new class to second folder (Migrations), name the new class: ConfigurationTwo. 
   Go to EyeQube on github, then to Lexicon_LMS --> folder Lexicon_LMS --> folder Migration --> open ConfigurationTwo.cs --> copy all written code --> paste all code to your newly created file: ConfigurationTwo.cs




6. Go to Package Manager Console and type:
   add-migration -ConfigurationTypeName Lexicon_LMS.Migrations.Configuration "InitialOne"   (then press ENTER) 




7. In package manager console, type: 
   update-database -ConfigurationTypeName Lexicon_LMS.Migrations.Configuration    (then press ENTER)




8. In package manager console, type: 
   add-migration -ConfigurationTypeName Lexicon_LMS.Migration.ConfigurationTwo "InitialTwo"    (then press ENTER)




9. Open the last migration file you added.. in this case:  InitialTwo.cs,  and code (write):  return;  before the first "CreateTable query" in the code.  
   (We can't create same tables twice, since we already have created this tables when adding migration "InitialOne")




10. In package manager console, type:
    update-database -ConfigurationTypeName Lexicon_LMS.Migration.ConfigurationTwo    (then press ENTER)


DONE! 


To run this application on your browser, hold down  "Ctrl"  and press "F5".
Once the application has opened on your browser, you can sign in as a administrator/teacher by using the following username and password:
*username: foo@bar.com
*password: foobar

In order to logg in as a student, you must first logg in as a teacher. Once you are logged in as a teacher, you can register a student (name, email adress and password) to a course.
Then you can sign out, and sign in again, but this time as the student you just created (hopefully you remember the students email and password).
 

*If you encounter problems during any of the "steps" (above), please contact me at: sattar.alvandpour@gmail.com , and I'll do my best to help you out.
 /Sattar Alvandpour




