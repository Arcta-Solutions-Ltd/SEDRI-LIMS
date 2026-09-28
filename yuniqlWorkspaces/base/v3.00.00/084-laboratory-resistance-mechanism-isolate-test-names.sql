-- Comma-separated isolate test names (culture test form names) allowed as resistance-mechanism tests for AST when non-empty; intersects with culture-type / organism-scope rules.
ALTER TABLE laboratory ADD resistancemechanismisolatetestnames VARCHAR(2000);

UPDATE laboratory SET resistancemechanismisolatetestnames = '' WHERE resistancemechanismisolatetestnames IS NULL;
