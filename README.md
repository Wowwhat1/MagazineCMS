# COMP1640

## Introduction: PublishPro

PublishPro is a web-based secure role-based system designed to streamline the process of collecting student contributions for an annual university magazine. Developed with the needs of a large university in mind, PublishPro offers a comprehensive platform for managing submissions, reviewing content, and facilitating communication between students, faculty members, and university administrators.

## Documentation

https://drive.google.com/drive/u/0/folders/1ofvQsIXHvXbw0isN5okT4mdhxmyQUKDQ

### Getting Started

1. **Prerequisites**: List any software, tools, or dependencies required to run your project.
2. **Installation**: Explain how to install and set up your project locally.
3. **Usage**: Describe how to use your project (commands, configuration, etc.).

### Outline Features

1. **Role-based Access Control**: The system implements role-based access control to ensure that each user, including the University Marketing Manager, Faculty Marketing Coordinators, students, and administrators, has appropriate permissions and access levels.

2. **Submission Management**: Students can submit one or more articles as Word documents, along with high-quality images, while adhering to agreed-upon Terms and Conditions. Submissions are automatically disabled after a closure date for new entries, with the option for updates until a final closure date.

3. **Faculty-specific Access**: Marketing Coordinators can only access contributions from students within their faculty, ensuring privacy and efficient management of submissions.

4. **University-wide Oversight**: The University Marketing Manager has access to view all selected contributions for the magazine, facilitating oversight and coordination across faculties.

5. **Statistical Analysis**: The system provides statistical analysis, such as the number of contributions per faculty, to offer insights into student engagement and participation.


### Contributing

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/my-feature`).
3. Make your changes and commit (`git commit -am 'Add my feature'`).
4. Push to the branch (`git push origin feature/my-feature`).
5. Create a new Pull Request.

NOTE:

Creating Branches:
 - Always create a new branch for each feature or bug fix.
 - Branches should be created off the main branch.
 - Use descriptive names for your branches, such as add-login-feature or fix-header-bug.
Working with Pull Requests:
 - Pull requests let you propose changes to a project.
 - They provide a platform for reviewing and discussing the proposed changes.
 - You can link a pull request to an issue to show that a fix is in progress.
 - After initializing a pull request, you can push commits from your topic branch to add them to your existing pull request.

### Git Convention

STRUCTURE OF A COMMIT MESSAGE

______________________
'Jira task code'/'type': 'description'

'body'
______________________

NOTE: 
 - type and description are required.
 - body is optional.

type:
 - feat: a new feature
 - fix: fix bug
 - docs: edit documents
 - style: add space, format code, ...
 - refactor: change method name, separate child method, delete redundancy code, ...
 - perf: improve performance
 - test: add test case, edit unit test, ...
 - build: changes that affect the build process
 - ci: change configure file or script CI

description:
 - Briefly describe the commit message
 - No longer than 50 characters
 - Use imperative sentences in the present tense
 - Do not capitalize the first letter
 - Do not use a period at the end of the sentence

body:
 - Is optional, used to describe more detail about the commit, if needed
 - Leave a blank line below <type>: <description>
 - Should be used to explain What or Why questions not How

### Migration Naming Convention

 - Descriptive Names: The migration name should describe the changes it makes to the database. For example, if a migration adds a Manager to Employees, could name it AddManagerToEmployees1
 - Use of Timestamps: To keep migrations in the order they were created, could prefix names with a timestamp, like 20220401_AddManagerToEmployees
 - Version Numbers: If maintaining versions of your database, could include the version number in the migration name, like v2_AddManagerToEmployees
 - Combination: Could also combine these strategies. For example, v2_20220401_AddManagerToEmployees

## License

Specify the license under which your project is released (e.g., MIT, Apache, etc.).
