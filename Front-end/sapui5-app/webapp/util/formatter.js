sap.ui.define([], function () {
  "use strict";

  return {
    formatarPaginacao: function (pagina, total) {
      if (!pagina || !total) {
        return "";
      }
      return "Página " + pagina + " de " + total;
    }
  };
});
