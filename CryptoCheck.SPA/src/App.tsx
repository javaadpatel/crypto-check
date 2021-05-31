import { Container } from "semantic-ui-react";
import { Switch, Route } from "react-router-dom";
import Home from "./Components/Home/Home";
import NavBar from "./Components/Shared/NavBar/NavBar";

function App(): JSX.Element {
  return (
    <Container>
      <NavBar />
      <Switch>
        <Route path={"/"} exact={true} component={Home} />
      </Switch>
    </Container>
  );
}

export default App;
