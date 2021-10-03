# Adding Profile Data

## Modify Database

1. Change model  
   File: Models -> IdentityModels -> ApplicationUser class  
   Add attribute  
   [check this link: line 13 - 15](http://github.com/claudiax/addingprofiledata/blob/main/addingprofiledata/Models/IdentityModels.cs)

   ```cs
   public class ApplicationUser : IdentityUser
   {
       public DateTime Dob { get; set; }  = DateTime.Parse("01 Jan 2000");
       public decimal Height { get; set; } = 0;
       public decimal Weight { get; set; } = 0;
       ...
   }
   ```

2. Add migration and update database  
   package manager console

   ```sh
   add-migration AddProfileData
   update-database
   ```

## Create View Model

- Add View Model  
   File: Model -> AccountViewModels  
   Add class ProfileViewModel inside namespace  
   [check this link: line 114 - 132](http://github.com/claudiax/addingprofiledata/blob/main/addingprofiledata/Models/AccountViewModels.cs)

  ```cs
  public class ProfileViewModel
  {
      public string Email { get; set; }

      [Required]
      [Display(Name = "Date of birth")]
      [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
      public DateTime Dob { get; set; };

      [Required]
      [Display(Name = "Height(m)")]
      [RegularExpression(@"^[0-2]+(\.\d{1,2})$", ErrorMessage = "Number with 2 decimal places.")]
      public decimal Height { get; set; };

      [Required]
      [Display(Name = "Weight (kg)")]
      [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Number with 2 decimal places.")]
      public decimal Weight { get; set; };
  }
  ```

## View

1.  Add link  
    File: Views -> Manage -> Index  
    [check this link: line 13 & 14](http://github.com/claudiax/addingprofiledata/blob/main/addingprofiledata/Views/Manage/Index.cshtml)

    ```html
    <dt>Profile</dt>
    <dd>@Html.ActionLink("View your profile", "ViewProfile")</dd>
    ```

2.  View profile  
     Create file: Views -> Manage -> ViewUserProfile.cshtml  
     [check this link for code](http://github.com/claudiax/addingprofiledata/blob/main/addingprofiledata/Views/Manage/ViewUserProfile.cshtml)

    ```html
    @model ChangeUserProfileSample.Models.ProfileViewModel @{ ViewBag.Title =
    "User Profile"; }

    <h2>User Profile</h2>

    <div>
      <dl class="dl-horizontal">
        <dt>@Html.DisplayNameFor(model => model.Email)</dt>
        <dd>@Html.DisplayFor(model => model.Email)</dd>
        <dt>@Html.DisplayNameFor(model => model.Dob)</dt>
        <dd>@Html.DisplayFor(model => model.Dob)</dd>
        <dt>@Html.DisplayNameFor(model => model.Height)</dt>
        <dd>@Html.DisplayFor(model => model.Height)</dd>
        <dt>@Html.DisplayNameFor(model => model.Weight)</dt>
        <dd>@Html.DisplayFor(model => model.Weight)</dd>
      </dl>
    </div>

    <p>@Html.ActionLink("Edit", "EditUserProfile", "Manage")</p>
    ```

3.  Edit profile  
    Create file: Views -> Manage -> EditUserProfile.cshtml  
    Remember to use date time picker  
    [check this link for code](http://github.com/claudiax/addingprofiledata/blob/main/addingprofiledata/Views/Manage/EditUserProfile.cshtml)

## Controller

File: Controllers -> ManageController

1. View User Profile  
   Add Method (This is a GET method, return view)  
    [check this link: line 326 - 346](http://github.com/claudiax/addingprofiledata/blob/main/addingprofiledata/Controllers/ManageController.cs)

   ```cs
   //
   // GET: /Manage/ViewUserProfile
   public async Task<ActionResult> ViewUserProfile()
   {
       try
       {
           var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
           var model = new ProfileViewModel {
               Email = user.Email,
               Dob = user.Dob,
               Height = user.Height,
               Weight = user.Weight
               };
           return View(model);
       }
       catch (Exception exception)
       {
           ModelState.AddModelError("Error", exception.Message);
           return HttpNotFound();
           }
   }
   ```

2. Edit User Profile (View)  
   Add Method (This is a GET Method, return view)  
   [check this link: line 348 - 369](http://github.com/claudiax/addingprofiledata/blob/main/addingprofiledata/Controllers/ManageController.cs)

   ```cs
   //
   // GET: /Manage/EditUserProfile
   public async Task<ActionResult> EditUserProfile()
   {
       try
       {
           var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
           var model = new ProfileViewModel
           {
               Email = user.Email,
               Dob = user.Dob,
               Height = user.Height,
               Weight = user.Weight
           };
           return View(model);
       }
       catch (Exception exception)
       {
           ModelState.AddModelError("Error", exception.Message);
           return HttpNotFound();
       }
   }
   ```

3. Edit User Profile  
   Add Method (This is a POST method, return ViewUserProfile)  
   [check this link: line 371 - 397](http://github.com/claudiax/addingprofiledata/blob/main/addingprofiledata/Controllers/ManageController.cs)

   ```cs
   //
   // POST: /Manage/EditUserProfile
   [HttpPost]
   [ValidateAntiForgeryToken]
   public async Task<ActionResult> EditUserProfile(ProfileViewModel model)
   {
       if (!ModelState.IsValid)
       {
           return View(model);
       }
       try
       {
           var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
           user.Dob = model.Dob;
           user.Height = model.Height;
           user.Weight = model.Weight;
           var result = await UserManager.UpdateAsync(user);
           if (!result.Succeeded)
               AddErrors(result);
           return RedirectToAction("ViewUserProfile");
       }
       catch(Exception exception)
       {
           ModelState.AddModelError("Error", exception.Message);
           return View(model);
       }
   }
   ```
