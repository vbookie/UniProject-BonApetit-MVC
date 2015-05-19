//#region Ingredients

var ingredientEntryTemplate = '<div class="col-md-10 ingredient-entry-row nopadding"><div class="col-md-5 nopadding"><input type="text" class="form-control ingredient-entry" /></div> ' +
    '<div class="col-md-5 btn-group nopadding" role="group">' +
    '<button class="btn btn-default" type="button" onclick="removeAdditionalIngredient(this)"><span class="glyphicon glyphicon-minus"></span></button>' +
    '<button class="btn btn-default" type="button" onclick="addAdditionalIngredient(this)"><span class="glyphicon glyphicon-plus"></span></button>' +
    '</div></div>';

function addAdditionalIngredient(button) {
    var ingredientEntryRow = $(button.parentNode.parentNode);
    ingredientEntryRow.after(ingredientEntryTemplate);

    increaseIngredientCount();
}

function removeAdditionalIngredient(button) {
    var ingredientEntryRow = $(button.parentNode.parentNode);
    ingredientEntryRow.remove();

    decreaseIngredientCount();
}

function increaseIngredientCount() {
    var ingredientsCount = $("#ingredientsCount");
    var newValue = Number(ingredientsCount.val()) + 1;
    ingredientsCount.val(newValue);
    ingredientsCount.change();
}

function decreaseIngredientCount() {
    var ingredientsCount = $("#ingredientsCount");
    var newValue = Number(ingredientsCount.val()) - 1;
    ingredientsCount.val(newValue);
    ingredientsCount.change();
}

function setupIngredients() {
    $("#ingredientsCount").change(function () {
        var value = Number($(this).val());
        if (value == 0) {
            $("#addIngredientButton").show();
        } else {
            $("#addIngredientButton").hide();
        }
    });
    $("#ingredientsCount").change();

    // main button
    $("#addIngredientButton").click(function () {
        $("#ingredientsList").append(ingredientEntryTemplate);
        increaseIngredientCount();
    });

    // set correct ids for the ingredients on submit
    $("#recipeForm").submit(function () {
        var ingredients = $(".ingredient-entry");
        var numberOfIngredients = ingredients.length;

        for (var i = 0; i < numberOfIngredients; i++) {
            ingredients[i].id = "Ingredients_" + i + "_";
            ingredients[i].name = "Ingredients[" + i + "]";
        }
    });
}

//#endregion

function setupImage() {
    var form;
    var dialogContainer = $("#addImageDialog");
    var dialog = dialogContainer.dialog({
        autoOpen: false,
        height: 400,
        width: 600,
        modal: true,
        buttons: {
            "Create": createImage,
            Cancel: function () {
                dialog.dialog("close");
            }
        },
        close: function () {
            dialog.empty();
        },
        show: {
            effect: "drop",
            duration: 1000
        },
        hide: {
            effect: "drop",
            duration: 1000
        }
    });

    function createImage() {
        var formData = new FormData(form[0]);

        $.ajax({
            url: '/Image/Create',
            type: 'POST',
            beforeSend: function () {
                $("progress").show();
            },
            success: function (result) {
                if (result.success === true) {
                    $("#recipeImageId").val(result.id);
                    $("#recipeImage").attr("src", result.url);
                    dialog.dialog("close");
                } else {
                    dialog.empty();
                    dialog.append(result.content);
                    styleFileInput();

                    form = dialogContainer.find("form");
                    form.on("submit", function (event) {
                        event.preventDefault();
                        createImage();
                    });
                }
            },
            xhr: function () {  // Custom XMLHttpRequest
                var myXhr = $.ajaxSettings.xhr();
                if (myXhr.upload) {
                    // myXhr.upload.addEventListener('progress', progressHandlingFunction, false);
                }
                return myXhr;
            },
            error: function (xhr, status, error) {
                dialog.empty();
                dialog.append(error);
            },
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        });
    }

    function styleFileInput() {
        $(":file").filestyle({ buttonBefore: true, input: false });
    }

    $("#chooseImageButton").click(function () {
        $.ajax({
            url: '/Image/Create',
            method: "GET",
            dataType: "html"
        }).success(function (data) {
            dialogContainer.append(data);
            styleFileInput();

            form = dialogContainer.find("form");
            form.on("submit", function (event) {
                event.preventDefault();
                createImage();
            });

            dialog.dialog("open");
        }).error(function (xhr, status, error) {
            dialogContainer.append(error);
            dialog.dialog("open");
        });
    });
}

function setupCategory() {
    var categoryForm;
    var catalogDialogContainer = $("#addCategoryDialog");
    var catalogDialog = catalogDialogContainer.dialog({
        autoOpen: false,
        height: 250,
        width: 400,
        modal: true,
        buttons: {
            "Create": createCategory,
            Cancel: function () {
                catalogDialog.dialog("close");
            }
        },
        close: function () {
            catalogDialog.empty();
        },
        show: {
            effect: "drop",
            duration: 1000
        },
        hide: {
            effect: "drop",
            duration: 1000
        }
    });

    function createCategory() {
        var formData = new FormData(categoryForm[0]);

        $.ajax({
            url: '/Category/Create',
            type: 'POST',
            beforeSend: function () {
            },
            success: function (result) {
                if (result.success === true) {
                    $("#categoriesList").append("<option value='" + result.id + "'>" + result.name + "</option>");
                    $("#categoriesList").selectpicker("refresh");
                    catalogDialog.dialog("close");
                } else {
                    catalogDialog.empty();
                    catalogDialog.append(result.content);

                    categoryForm = catalogDialogContainer.find("form");
                    categoryForm.on("submit", function (event) {
                        event.preventDefault();
                        createCategory();
                    });
                }
            },
            xhr: function () {  // Custom XMLHttpRequest
                var myXhr = $.ajaxSettings.xhr();
                if (myXhr.upload) {

                }
                return myXhr;
            },
            error: function (xhr, status, error) {
                catalogDialog.empty();
                catalogDialog.append(error);
            },
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        });
    }

    $("#addCategoryButton").click(function () {
        $.ajax({
            url: '/Category/Create',
            method: "GET",
            dataType: "html"
        }).success(function (data) {
            catalogDialogContainer.append(data);

            categoryForm = catalogDialogContainer.find("form");
            categoryForm.on("submit", function (event) {
                event.preventDefault();
                createCategory();
            });

            catalogDialog.dialog("open");
        }).error(function (xhr, status, error) {
            catalogDialogContainer.append(error);
            catalogDialog.dialog("open");
        });
    });
}



$(document).ready(function () {
    setupIngredients();
    setupImage();

    $('#categoriesList').selectpicker();
    setupCategory();
});