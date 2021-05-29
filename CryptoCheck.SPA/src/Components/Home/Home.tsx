import React, { useEffect, useState } from "react";
import { Grid, Segment } from "semantic-ui-react";
import QuoteForm from "./QuoteForm";
import QuoteInfoCard from "./QuoteInfoCard";

function Home() {
  return (
    <Segment fluid padded="very" basic>
      <Grid stackable divided="vertically">
        <Grid.Row columns={2}>
          <Grid.Column>
            <QuoteForm />
          </Grid.Column>
          <Grid.Column>
            <QuoteInfoCard />
          </Grid.Column>
        </Grid.Row>
      </Grid>
    </Segment>
  );
}

export default Home;
