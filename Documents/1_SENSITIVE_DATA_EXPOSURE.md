1 Sensitive Data Exposure
=========================
[Sensitive Data Exposure](https://owasp.org/www-project-top-ten/OWASP_Top_Ten_2017/Top_10-2017_A3-Sensitive_Data_Exposure) is a broad topic that covers everything from exposing unsecured databases directly online to losing memory sticks with sensitive data. One common risk for developers, is to commit sensitive data alongside or embedded in the code to a public git-repository.

The Fault
---------
We have an application, but we don't have an account to login with. Let's start by finding one.

In .NET projects, configuration like database access keys and other passwords are usually stored in a file called [appsettings.json](../Notes.Api/appsettings.json). Seems like someone forgot to remove the password for the "Test"-user. Whoops!

The Fix
-------
The only good fix for exposed passwords and secrets is unfortunately to change them, and evaluate the potential loss of sensitive data. Since you don't have access to change the password used on the server, we unfortunately have to leave this issue as-is, and grind our teeth. You can of course change the password locally on your machine if you wish. If you do, you probably have to restart Notes.Api in order for the configuration change to take effect.

_There are several things you can do to reduce the chance of checking in passwords and other secrets. One is to make sure that all sensitive data is configurable from a development settings-file, like [appsettings.development.json](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#default-configuration), that can be added to the .gitignore. Additional tips can be found at [GitHub](https://help.github.com/en/github/authenticating-to-github/removing-sensitive-data-from-a-repository#avoiding-accidental-commits-in-the-future)._

The Flag
--------
The first flag has been removed from the code long ago. But can you still find it?

_Please use the same name for all your submissions._

<form id="answer-form">
    <input name="flag" type="hidden" value="SENSITIVE_DATA_EXPOSURE" />
    <input name="name" type="text" maxlength="100" placeholder="Your name" />
    <input name="answer" type="text" maxlength="100" placeholder="The solution!" />
    <button type="submit" id="submit-answer">Submit</button>
</form>
<p id="wrong-answer" hidden>The solution is wrong</p>
<p id="correct-answer" hidden>The solution is correct!</p>
<script>
const answerForm = document.getElementById("answer-form");
const taskInput = answerForm.elements["flag"];
const nameInput = answerForm.elements["name"];
const answerInput = answerForm.elements["answer"];
const correctAnswer = document.getElementById("correct-answer");
const wrongAnswer = document.getElementById("wrong-answer");

answerForm.onsubmit = event => {
  event.preventDefault();

  const answer = {
    task: taskInput.value,
    name: nameInput.value,
    answer: answerInput.value
  };

  fetch("https://sticky-notes.eu.ngrok.io/Answers", {
    method: "POST",
    body: JSON.stringify(answer),
    headers: { "Content-Type": "application/json" }
  })
    .then(res => res.json())
    .then(_ => {
        wrongAnswer.hidden = true;
        correctAnswer.hidden = false;
    })
    .catch((error) => {
        // console.error('Error:', error);
        wrongAnswer.hidden = false;
        correctAnswer.hidden = true;
    });
};
</script>
