(window.webpackJsonp=window.webpackJsonp||[]).push([[0],{10:function(e,t,a){e.exports=a(15)},15:function(e,t,a){"use strict";a.r(t);var n=a(0),s=a.n(n),i=a(9),r=a.n(i),c=a(2),o=a(3),l=a(6),h=a(4),d=a(5),u=a(7),m=a(1),b=function(e){return s.a.createElement("div",{className:"field"},s.a.createElement("label",{className:"field-checkbox",htmlFor:"".concat(e.name,"-").concat(e.value)},e.title,s.a.createElement("input",{id:"".concat(e.name,"-").concat(e.value),name:e.name,type:"checkbox",value:e.value,checked:e.checked,onChange:e.handleChange}),s.a.createElement("span",{className:"checkmark"})))},g=function(e){return s.a.createElement("div",{className:"field"},s.a.createElement("label",{htmlFor:e.name},e.title),s.a.createElement("input",{id:e.name,type:e.type,name:e.name,value:e.value,onChange:e.handleChange}))},p=function(e){var t;return void 0===(t=void 0!==e.messages&&e.messages.length>0?s.a.createElement("ul",null,e.messages.map(function(e,t){return s.a.createElement("li",{key:t},e)})):e.message)||null===t||0===t.length?null:s.a.createElement("div",{className:"text-danger validation-summary-errors"},t)},v=function(){return document.getElementById("RequestVerificationToken").value},f=function(e){function t(e){var a;return Object(c.a)(this,t),(a=Object(l.a)(this,Object(h.a)(t).call(this,e))).handleChange=a.handleChange.bind(Object(m.a)(Object(m.a)(a))),a.handleSubmit=a.handleSubmit.bind(Object(m.a)(Object(m.a)(a))),a.state={databaseServer:"",databaseName:"",integratedSecurity:!1,databaseLogin:"",databasePassword:"",errors:[]},a}return Object(d.a)(t,e),Object(o.a)(t,[{key:"handleChange",value:function(e){var t,a=e.target,n="checkbox"===a.type?a.checked:a.value,s=a.name;this.setState((t={},Object(u.a)(t,s,n),Object(u.a)(t,"displaySuccess",!1),t))}},{key:"handleSubmit",value:function(e){var t=this;e.preventDefault();var a=[];if(0===this.state.databaseServer.length&&a.push("Database server can't be empty"),0===this.state.databaseName.length&&a.push("Database name can't be empty"),this.state.integratedSecurity||(0===this.state.databaseLogin.length&&a.push("Database login can't be empty"),0===this.state.databasePassword.length&&a.push("Database password can't be empty")),a.length>0)this.setState({errors:a});else{this.props.setLoading(!0,"Saving database ...");var n={};n.databaseServer=this.state.databaseServer,n.databaseName=this.state.databaseName,n.databaseLogin=this.state.databaseLogin,n.databasePassword=this.state.databasePassword,n.integratedSecurity=this.state.integratedSecurity,fetch("/install/database/",{method:"POST",credentials:"include",headers:{Accept:"application/json","Content-Type":"application/json",RequestVerificationToken:v()},body:JSON.stringify(n)}).then(function(e){e.ok?setTimeout(function(){return t.props.checkDatabase()},4e3):e.json().then(function(e){a.push(e),t.setState({errors:a},function(){t.props.setLoading(!1,"")})})}).catch(function(e){console.log("Error: \n",e.message)})}}},{key:"renderCredentials",value:function(){if(!this.state.integratedSecurity)return s.a.createElement("div",null,s.a.createElement(g,{type:"text",name:"databaseLogin",title:"Database login",value:this.state.password,handleChange:this.handleChange}),s.a.createElement(g,{type:"password",name:"databasePassword",title:"Database password",value:this.state.password,handleChange:this.handleChange}))}},{key:"render",value:function(){return this.props.display?s.a.createElement("form",{onSubmit:this.handleSubmit},s.a.createElement(p,{messages:this.state.errors}),s.a.createElement(g,{type:"text",name:"databaseServer",title:"Database server",value:this.state.email,handleChange:this.handleChange}),s.a.createElement(g,{type:"text",name:"databaseName",title:"Database name",value:this.state.email,handleChange:this.handleChange}),s.a.createElement(b,{name:"integratedSecurity",title:"Integrated security",checked:this.state.integratedSecurity,handleChange:this.handleChange}),this.renderCredentials(),s.a.createElement("div",{className:"actions"},s.a.createElement("button",{type:"submit",className:"button button-primary"},"Validate"))):null}}]),t}(n.Component),y=function(e){function t(e){var a;return Object(c.a)(this,t),(a=Object(l.a)(this,Object(h.a)(t).call(this,e))).handleChange=a.handleChange.bind(Object(m.a)(Object(m.a)(a))),a.handleSubmit=a.handleSubmit.bind(Object(m.a)(Object(m.a)(a))),a.state={email:"",password:"",confirmPassword:"",errors:[]},a}return Object(d.a)(t,e),Object(o.a)(t,[{key:"handleChange",value:function(e){var t,a=e.target,n="checkbox"===a.type?a.checked:a.value,s=a.name;this.setState((t={},Object(u.a)(t,s,n),Object(u.a)(t,"displaySuccess",!1),t))}},{key:"handleSubmit",value:function(e){var t=this;e.preventDefault();var a=[];if(0===this.state.email.length&&a.push("Email can't be empty"),0===this.state.password.length&&a.push("Password can't be empty"),this.state.password!==this.state.confirmPassword&&a.push("Password fields must be equal"),a.length>0)this.setState({errors:a});else{this.props.setLoading(!0,"Creating account ...");var n={};n.email=this.state.email,n.password=this.state.password,n.confirmPassword=this.state.confirmPassword,fetch("/install/account",{method:"POST",credentials:"include",headers:{Accept:"application/json","Content-Type":"application/json",RequestVerificationToken:v()},body:JSON.stringify(n)}).then(function(e){e.ok?document.location.href="/":e.json().then(function(e){if(Array.isArray(e))for(var n=0;n<e.length;n++)a.push(e[n].description);else a.push(e);t.setState({errors:a},function(){t.props.setLoading(!1,"")})})}).catch(function(e){console.log("Error: \n",e.message)})}}},{key:"render",value:function(){return this.props.display?s.a.createElement("form",{onSubmit:this.handleSubmit},s.a.createElement(p,{messages:this.state.errors}),s.a.createElement(g,{type:"text",name:"email",title:"Email",value:this.state.email,handleChange:this.handleChange}),s.a.createElement(g,{type:"password",name:"password",title:"Password",value:this.state.password,handleChange:this.handleChange}),s.a.createElement(g,{type:"password",name:"confirmPassword",title:"Confirm password",value:this.state.confirmPassword,handleChange:this.handleChange}),s.a.createElement("div",{className:"actions"},s.a.createElement("button",{type:"submit",className:"button button-primary"},"Install"))):null}}]),t}(n.Component),E=function(e){return e.isLoading?s.a.createElement("div",{className:"loader-component"},s.a.createElement("div",{className:"loader-content"},s.a.createElement("div",{className:"loader"},s.a.createElement("div",{className:"inner one"}),s.a.createElement("div",{className:"inner two"}),s.a.createElement("div",{className:"inner three"})),s.a.createElement("p",{className:"message"},e.message))):null},k=function(e){function t(){var e;return Object(c.a)(this,t),(e=Object(l.a)(this,Object(h.a)(t).call(this))).state={databaseExists:!1,accountCreateMode:!1,isLoading:!1,message:!1},e}return Object(d.a)(t,e),Object(o.a)(t,[{key:"componentDidMount",value:function(){this.checkDatabase()}},{key:"checkDatabase",value:function(){var e=this;this.setState({isLoading:!0,message:"Checking database..."}),fetch("/install/checkdatabase",{method:"GET"}).then(function(t){t.ok?(e.setState({isLoading:!1,databaseExists:!0}),e.checkDatabaseTables()):e.setState({isLoading:!1,databaseExists:!1})}).catch(function(e){this.setState({isLoading:!1,databaseExists:!1})})}},{key:"checkDatabaseTables",value:function(){var e=this;this.setState({isLoading:!0,message:"Checking database tables..."}),fetch("/install/checkdatabasetables",{method:"GET"}).then(function(t){t.ok?e.setState({isLoading:!1,accountCreateMode:!1}):e.setState({isLoading:!1,accountCreateMode:!0})}).catch(function(e){console.log(e)})}},{key:"setLoading",value:function(e,t){this.setState({isLoading:e,message:t})}},{key:"render",value:function(){var e=this,t=this.state.isLoading;return s.a.createElement("div",null,s.a.createElement("h2",{className:"title"},"Kastra - Installation"),s.a.createElement("hr",null),s.a.createElement("h3",{className:"subtitle"},"Configure your website."),s.a.createElement(E,{isLoading:this.state.isLoading,message:this.state.message}),s.a.createElement(f,{display:!t&&!this.state.databaseExists,checkDatabase:function(){return e.checkDatabase()},setLoading:function(t,a){return e.setLoading(t,a)}}),s.a.createElement(y,{display:!t&&this.state.databaseExists&&this.state.accountCreateMode,setLoading:function(t,a){return e.setLoading(t,a)}}))}}]),t}(n.Component);Boolean("localhost"===window.location.hostname||"[::1]"===window.location.hostname||window.location.hostname.match(/^127(?:\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$/));r.a.render(s.a.createElement(k,null),document.getElementsByClassName("content")[0]),"serviceWorker"in navigator&&navigator.serviceWorker.ready.then(function(e){e.unregister()})}},[[10,2,1]]]);