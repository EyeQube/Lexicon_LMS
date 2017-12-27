
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

* Add/Remove/Edit the schedule with events/activites 
  (each course has their own schedule, and students can only view the schedule, but they can't modify it)
* Register/Remove/Edit students
* Register/Remove/Edit courses
* Add/Remove/Edit modules that belongs to a course
* Add/Remove/Edit activities belonging to a module
* Upload and download files


Step by step instructions for how you run the application (10 steps)
--------------------------------------------------------------------

1. Make sure your Migrations folder is empty and that you don't have any database connection in App_Data folder. 
   If not, delete all your current migrations files within the Migrations folder. Then
   go to "SQL Server Object Explorer", delete the database connection for this project. Go to App_Data folder in "Solutions    
   Explorer" and delete the database connection file (mdf file).  
   



2. Install DHTMLX Scheduler (*30 day free trial), and Entity Framework.
   Right click on your project in the Solution Explorer and select Manage NuGet Packages. 
   It will open the Manage NuGet Packages dialog box.




3. Now select the Online option in the left bar and search for DHTMLX Scheduler.NET and EntityFramework, and install them both.
 



4. This project needs two configuration files. Each configuration file is supposed to be in their own Migrations folder. 
   One folder (Migrations) already exists, therefore you create only the second migration folder, name it: "Migration". 
   *Note: Second folder does NOT end with letter "s". Hence name Migration, not Migration(s).
   



5. Now you must start enabling  ScheduleContext.cs first, since SchedulerContext.cs inherits from ApplicationDbContext.cs! 
   For that, go to Package Manager Console and type and run:
   
   Enable-Migrations -ContextTypeName Lexicon_LMS.Models.SchedulerContext      




6. Add a new class to second folder (Migrations), name the new class: ConfigurationTwo. 
   Go to: https://github.com/EyeQube/Lexicon_LMS/tree/master/Lexicon_LMS/Migration ,
   and open ConfigurationTwo.cs and copy all written code in that file. 
   Paste all copied code to your newly locally created ConfigurationTwo.cs file.




7. In Package Manager Console, type and run:

   add-migration -ConfigurationTypeName Lexicon_LMS.Migrations.Configuration "InitialOne"    




8. In Package Manager Console, type and run: 

   update-database -ConfigurationTypeName Lexicon_LMS.Migrations.Configuration    




9. In Package Manager Console, type and run: 

   add-migration -ConfigurationTypeName Lexicon_LMS.Migration.ConfigurationTwo "InitialTwo"    




10. Open the migration file (InitialTwo.cs) that you just added, and code (write):  return;  
   before the first "CreateTable query" in the code.  
   (We can't create same tables twice since we already have created this tables when we added migration "InitialOne")




11. In Package Manager Console, type and run:

    update-database -ConfigurationTypeName Lexicon_LMS.Migration.ConfigurationTwo    


DONE! 


To run this application now on your browser, hold down  "Ctrl"  and press "F5".
Once the application has opened on your browser, you can sign in as a administrator/teacher by using the following username and password:
*username: foo@bar.com
*password: foobar

In order to sign in as a student (assuming you're neither a student yet, nor know/remember any registered students email adress and password), you must first sign in as a teacher. Once you are signed in as a teacher, 
register a student (name, email adress and password) to a course (make sure you remember the email adress and password).
Then sign out, and sign in with the email adress and password that belongs to the student you just created 
(hopefully you remember the email adress and password).
 

*If you encounter problems during any of the "steps" (above), please contact me: sattar.alvandpour@gmail.com
and I'll do my best to help you out.  

/Sattar Alvandpour




