//var clarifaiApiKey = '4a4aad6a58de461a8bd2357d451cada5';
var clarifaiApiKey = 'c191a51fc9764331af16fc0bf4995dd2';
var workflowId = 'task7workflowID';

var app = new Clarifai.App({
    apiKey: clarifaiApiKey
});

// Handles image upload
function uploadImage() {
    var preview = document.querySelector('img');
    var file = document.querySelector('input[type=file]').files[0];
    var reader = new FileReader();

    reader.addEventListener("load", function () {
        preview.src = reader.result;
        var imageData = reader.result;
        imageData = imageData.replace(/^data:image\/(.*);base64,/, '');
        predictFromWorkflow(imageData);
    }, false);

    if (file) {
        reader.readAsDataURL(file);
        preview.style.display = "inherit";
    }
}

// Analyzes image provided with Clarifai's Workflow API
function predictFromWorkflow(photoUrl) {
    app.workflow.predict(workflowId, { base64: photoUrl }).then(
        function (response) {
            var outputs = response.results[0].outputs;
            var analysis = $(".analysis");

            analysis.empty();
            console.log(outputs);

            outputs.forEach(function (output) {
                var modelName = getModelName(output);

                // Create heading for each section
                var newModelSection = document.createElement("div");
                newModelSection.className = modelName + " modal-container";

                var newModelHeader = document.createElement("h2");
                newModelHeader.innerHTML = modelName;
                newModelHeader.className = "model-header";

                var formattedString = getFormattedString(output);
                var newModelText = document.createElement("p");
                newModelText.innerHTML = formattedString;
                newModelText.className = "model-text";

                newModelSection.append(newModelHeader);
                newModelSection.append(newModelText);
                analysis.append(newModelSection);
            });
        },
        function (err) {
            console.log(err);
        }
    );
}

// Helper function to get model name
function getModelName(output) {
    if (output.model.display_name !== undefined) {
        return output.model.display_name;
    } else if (output.model.name !== undefined) {
        return output.model.name;
    } else {
        return "";
    }
}

// Helper function to get output customized for each model
function getFormattedString(output) {
    var formattedString = "";
    var data = output.data;
    var maxItems = 3;
    // General
    if (output.model.model_version.id === "aa9ca48295b37401f8af92ad1af0d91d") {
        var items = data.concepts;
        if (items.length < maxItems) {
            maxItems = items.length;
            if (maxItems === 1) {
                formattedString = "The thing we are most confident in detecting is:";
            }
        } else {
            formattedString = "The " + maxItems + " things we are most confident in detecting are:";
        }

        for (var i = 0; i < maxItems; i++) {
            formattedString += "<br/>- " + items[i].name + " at a " + (Math.round(items[i].value * 10000) / 100) + "% probability";
        }
    }
    // Apparel 
    else if (output.model.model_version.id === "dc2cd6d9bff5425a80bfe0c4105583c1") {
        var items = data.concepts;
        if (items.length < maxItems) {
            maxItems = items.length;
            if (maxItems === 1) {
                formattedString = "The piece of apparel we are most confident in detecting is:";
            }
        } else {
            formattedString = "The " + maxItems + " pieces of apparel we are most confident in detecting are:";
        }

        for (var i = 0; i < maxItems; i++) {
            formattedString += "<br/>- " + items[i].name + " at a " + (Math.round(items[i].value * 10000) / 100) + "% probability";
        }
    }
    // Celebrity
    else if (output.model.model_version.id === "bdb0537982ae4e0da563ed836ccfa065") {
        var items = data.regions;
        if (data.regions.length === 1) {
            formattedString = "The person in this picture we are confident in detecting is:<br/>";
        } else {
            formattedString = "The people in this picture we are confident in detecting are:<br/>";
        }

        for (var i = 0; i < items.length; i++) {
            var item = items[i].data.face.identity.concepts[0];
            formattedString += "- " + item.name + " at a " + (Math.round(item.value * 10000) / 100) + "% probability<br/>";
        }
    }
    // Color
    else if (output.model.model_version.id === "dd9458324b4b45c2be1a7ba84d27cd04") {
        var items = data.colors;
        if (items.length < maxItems) {
            maxItems = items.length;
            if (maxItems === 1) {
                formattedString = "The color we are most confident in detecting is:";
            }
        } else {
            formattedString = "The " + maxItems + " colors we are most confident in detecting are:";
        }

        for (var i = 0; i < maxItems; i++) {
            formattedString += "<br/>- " + items[i].raw_hex + " (" + items[i].w3c.name + ") at a " + (Math.round(items[i].value * 10000) / 100) + "% probability";
        }
    }
    // Demographics
    else if (output.model.model_version.id === "f783f0807c52474c8c6ad20c8cf45fc0") {
        var items = data.regions;
        formattedString = "The demographics we are confident in detecting are:";

        for (var i = 0; i < items.length; i++) {
            var item = items[i].data.face;
            formattedString += "<br/>- " + item.multicultural_appearance.concepts[0].name + ", "
                + item.gender_appearance.concepts[0].name + ", "
                + item.age_appearance.concepts[0].name + " year old";
        }
    }
    // Face Detection
    else if (output.model.model_version.id === "c67b5872d8b44df4be55f2b3de3ebcbb") {
        var numFaces = data.regions.length;
        if (numFaces === 1) {
            formattedString = "There is 1 face detected in this picture.";
        } else {
            formattedString = "There are " + numFaces + " faces detected in this picture.";
        }
    }
    // Face Embedding
    else if (output.model.model_version.id === "ec1740642c83478392e7b8735c43c630") {
        var items = data.regions;
        if (items.length === 1) {
            formattedString = "Open up the console to see an array of numerical vectors representing 1 face in a 1024-dimensional space.";
        } else {
            formattedString = "Open up the console to see " + items.length + " arrays of numerical vectors representing " + items.length + " faces in a 1024-dimensional space.";
        }
        for (var i = 0; i < items.length; i++) {
            console.log("*** Face Embedding Output ***");
            console.log("Face " + i);
            console.log(items[i].data.embeddings[0]);
        }
    }
    // Focus
    else if (output.model.model_version.id === "fefeafd0c9224bce9274f06dad43553e") {
        formattedString = "Tis image has:<br/>- focus value of " + data.focus.value + "<br/>- density of " + data.focus.density;
    }
    // Food
    else if (output.model.model_version.id === "dfebc169854e429086aceb8368662641") {
        var items = data.concepts;
        if (items.length < maxItems) {
            maxItems = items.length;
            if (maxItems === 1) {
                formattedString = "The " + maxItems + " food item we are most confident in detecting are:";
            }
        } else {
            formattedString = "The " + maxItems + " food items we are most confident in detecting are:";
        }

        for (var i = 0; i < maxItems; i++) {
            formattedString += "<br/>- " + items[i].name + " at a " + (Math.round(items[i].value * 10000) / 100) + "% probability";
        }
    }
    // General Embedding
    else if (output.model.model_version.id === "bb7ac05c86be42d38b67bc473d333e07") {
        formattedString = "Open up the console to see an array of numerical vectors representing the input image in a 1024-dimensional space.";
        console.log("*** General Embedding Output ***");
        console.log(data.embeddings[0]);
    }
    // Landscape Quality
    else if (output.model.model_version.id === "a008c85bb6d44448ad35470bcd22666c") {
        var items = data.concepts;
        formattedString = "The probability that this photo's landscape is:";
        for (var i = 0; i < items.length; i++) {
            formattedString += "<br/>- " + items[i].name + " is " + (Math.round(items[i].value * 10000) / 100) + "%";
        }
    }
    // Logo
    else if (output.model.model_version.id === "ef1b7237d28b415f910ca343a9145e99") {
        var items = data.regions;
        if (items.length < maxItems) {
            maxItems = items.length;
            if (maxItems === 1) {
                formattedString = "The " + maxItems + " logos we are most confident in detecting are:";
            }
        } else {
            formattedString = "The " + maxItems + " logos we are most confident in detecting are:";
        }

        for (var i = 0; i < maxItems; i++) {
            formattedString += "<br/>- " + items[i].data.concepts[0].name + " at a " + (Math.round(items[i].data.concepts[0].value * 10000) / 100) + "% probability";
        }
    }
    // Moderation
    else if (output.model.model_version.id === "aa8be956dbaa4b7a858826a84253cab9") {
        var items = data.concepts;
        formattedString = "This photo is/contains:";
        for (var i = 0; i < items.length; i++) {
            formattedString += "<br/>- " + items[i].name + " at a " + (Math.round(items[i].value * 10000) / 100) + "% probability";
        }
    }
    // NSFW
    else if (output.model.model_version.id === "aa47919c9a8d4d94bfa283121281bcc4") {
        var items = data.concepts;
        formattedString = "This photo is:";
        for (var i = 0; i < items.length; i++) {
            formattedString += "<br/>- " + items[i].name + " at a " + (Math.round(items[i].value * 10000) / 100) + "% probability";
        }
    }
    // Portrait Quality
    else if (output.model.model_version.id === "c2e2952acb80429c8abb53e2fe3e11cd") {
        var items = data.concepts;
        formattedString = "The probability that this photo's portraits are:";
        for (var i = 0; i < items.length; i++) {
            formattedString += "<br/>- " + items[i].name + " is " + (Math.round(items[i].value * 10000) / 100) + "%";
        }
    }
    // Textures & Patterns
    else if (output.model.model_version.id === "b38274b04b1b4fb28c1b442dbfafd1ef") {
        var items = data.concepts;
        if (items.length < maxItems) {
            maxItems = items.length;
            if (maxItems === 1) {
                formattedString = "The texture or pattern we are most confident in detecting is:";
            }
        } else {
            formattedString = "The " + maxItems + " textures and/or patterns we are most confident in detecting are:";
        }

        for (var i = 0; i < maxItems; i++) {
            formattedString += "<br/>- " + items[i].name + " at a " + (Math.round(items[i].value * 10000) / 100) + "% probability";
        }
    }
    // Travel
    else if (output.model.model_version.id === "d2ffbf9730fd41fea79063270847be82") {
        var items = data.concepts;
        if (items.length < maxItems) {
            maxItems = items.length;
            if (maxItems === 1) {
                formattedString = "The travel topic we are most confident in detecting is:";
            }
        } else {
            formattedString = "The " + maxItems + " travel topics we are most confident in detecting are:";
        }

        for (var i = 0; i < maxItems; i++) {
            formattedString += "<br/>- " + items[i].name + " at a " + (Math.round(items[i].value * 10000) / 100) + "% probability";
        }
    }
    // Wedding
    else if (output.model.model_version.id === "b91bcf877c464a38a25a742694da7535") {
        var items = data.concepts;
        if (items.length < maxItems) {
            maxItems = items.length;
            if (maxItems === 1) {
                formattedString = "The wedding topic we are most confident in detecting is:";
            }
        } else {
            formattedString = "The " + maxItems + " wedding topics we are most confident in detecting are:";
        }

        for (var i = 0; i < maxItems; i++) {
            formattedString += "<br/>- " + items[i].name + " at a " + (Math.round(items[i].value * 10000) / 100) + "% probability";
        }
    }

    return formattedString;
}