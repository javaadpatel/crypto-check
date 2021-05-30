import { Formik } from "formik";
import { Form, Button, Message, Input } from "semantic-ui-react";
import * as yup from "yup";
import _ from "lodash";
import { useCryptoCheck } from "../../../Contexts/CryptoCheck-Context";

const QuoteForm = () => {
  const { getQuote } = useCryptoCheck();

  return (
    <Formik
      initialValues={{
        symbol: "",
      }}
      validationSchema={yup.object().shape({
        symbol: yup
          .string()
          .required("You must enter a cryptocurrency symbol")
          .max(4, "Cryptocurrency symbol cannot be more than 4 characters")
          .matches(/^[A-Za-z]+$/, "Only letters are allowed"),
      })}
      onSubmit={async (values, actions) => {
        await getQuote(values.symbol);
        actions.setSubmitting(false);
      }}
      render={({
        errors,
        handleSubmit,
        isSubmitting,
        dirty,
        setFieldValue,
        getFieldProps,
        touched,
        values,
      }) => (
        <>
          {touched.symbol && errors.symbol && (
            <Message
              data-testid="symbolError"
              error
              content={errors.symbol}
              attached
            />
          )}
          <Form loading={isSubmitting} onSubmit={handleSubmit}>
            <Form.Input
              required
              id="symbol"
              label="Which cryptocurrency do you want to check the price of?"
              {...getFieldProps("symbol")}
              placeholder="BTC"
            />
            <Button
              primary
              type="submit"
              loading={isSubmitting}
              disabled={isSubmitting || !_.isEmpty(errors) || !dirty}
              floated="right"
            >
              Get quote
            </Button>
          </Form>
        </>
      )}
    />
  );
};

export default QuoteForm;
