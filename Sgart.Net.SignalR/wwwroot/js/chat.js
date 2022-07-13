"use strict";

const htmlEncode = msg => {
  if (msg === undefined || msg === null)
    return null;
  var node = document.createTextNode(msg);
  return document.createElement("a").appendChild(node).parentNode.innerHTML.replace(/'/g, "&#39;").replace(/"/g, "&#34;");
};

try {
  //Disable the send button until connection is established.
  document.getElementById("send-button").disabled = true;

  const clientUniqueId = Math.floor(Math.random() * 999999999) + "-" + Date.now();  // TODO: da migliorare

  document.getElementById("user-input").value = "User " + Math.floor(Math.random() * 9999);

  //---------------------------------------------------------------------------------------
  // setup chat

  const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

  connection.start()
    .then(() => document.getElementById("send-button").disabled = false)
    .catch(err => console.error(err.toString()));


  // receive message

  connection.on("ReceiveMessage", data => {
    try {
      const user = htmlEncode(data.user);
      const date = htmlEncode(data.date);
      const message = htmlEncode(data.message);

      const template = `<div class="msg-header"><span class="msg-user">${user}</span><span class="msg-date">${date}</span></div>`
        + `<div class="msg-body"><blockquote>${message}</blockquote></div>`;

      const wrapper = document.createElement("div");
      wrapper.className = data.clientId === clientUniqueId ? "msg msg-my" : "msg msg-others";
      wrapper.innerHTML = template;

      const wrapperMessages = document.getElementById("messages-list");
      
      wrapperMessages.appendChild(wrapper);

      // scroll bottom to show last message
      wrapperMessages.scrollTop = wrapperMessages.scrollHeight;

    } catch (ex) {
      console.error(ex, "ReceiveMessage");
    }
  });


  // send message

  const sendMessage = event => {
    event.preventDefault();
    try {
      if (document.getElementById("send-button").disabled === true) {
        return;
      }

      const elmMessage = document.getElementById("message-input");

      if (elmMessage.value.length === 0) {
        return;
      }

      const data = {
        clientId: clientUniqueId,
        user: document.getElementById("user-input").value,
        message: elmMessage.value
      };



      // SendMessage = nome del metodo nella classe Hub
      connection.invoke("SendMessage", data)
        .catch(err => console.error(err.toString()));

      elmMessage.value = "";
      elmMessage.focus();
    } catch (ex) {
      console.error(ex, "sendMessage");
    }

  };

  // aggancio l'evento di submit al form
  document.getElementById("message-form").addEventListener("submit", sendMessage);

} catch (ex) {
  console.error(ex, "chat");

}
