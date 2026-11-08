const app1 = Vue.createApp({
    data() {
        return {
            isUploading: false,
            fileName: "",
            progress: ""
        }
    },
    methods: {
        changeName(event) {
            this.fileName = event.target.files[0].name;
        },
        uploadFile(event) {
            this.isUploading = true;
            var xhr = new XMLHttpRequest();
            xhr.timeout = -1
            var formData = new FormData();

            xhr.upload.onprogress = (event) => {
                let prog = Math.round((event.loaded / event.total) * 100);
                if(prog < 100){
                    this.progress = prog + "%";
                }
                else{
                    this.progress = "File loaded. Processing...";
                }
            }

            xhr.onload = xhr.onerror = function (e) {
                window.location = xhr.responseURL;
                console.log(e);
            }

            xhr.open("POST", "/file/upload");

            formData.append("fileName", event.target[1].value);
            formData.append("isPublic", event.target[2].checked);
            formData.append("file", event.target[0].files[0]);

            console.log(formData.get("isPublic"));

            xhr.send(formData);
        }
    }
});

app1.mount("#app1");