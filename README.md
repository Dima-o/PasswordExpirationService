## Author
Dmitry Evdokimov, student at Reshetnev Siberian State University (SibSU).
Designed the architecture and implemented the prototype during a production internship at Orion Telecom LLC.
The solution was then handed over to the company and further developed by its team.

## Status
Prototype. Known limitations: the new password is sent by email and stored in a JSON file,
and there is no structured logging or unit tests. A production version should use a password reset link
instead of sending the password, force a password change at next logon, add ILogger-based logging,
and include unit tests.
