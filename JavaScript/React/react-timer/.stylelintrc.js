module.exports = {
  extends: ['stylelint-config-standard'],
  plugins: ['stylelint-order'],
  rules: {
    'order/properties-alphabetical-order': true,

    'no-descending-specificity': null,
    'font-family-no-missing-generic-family-keyword': null,
  },
};