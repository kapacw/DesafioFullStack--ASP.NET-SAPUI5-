sap.ui.define(["sap/ui/model/json/JSONModel"], function (JSONModel) {
  "use strict";

  return {
    createTaskModel: function () {
      return new JSONModel({
        items: [],
        page: 1,
        pageSize: 10,
        title: "",
        sort: "title",
        order: "asc"
      });
    }
  };
});
